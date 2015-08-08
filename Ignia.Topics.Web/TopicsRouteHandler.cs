/*==============================================================================================================================
| Author        Jeremy Caney, Ignia LLC
| Client        Ignia, LLC
| Project       Topics Library
|
| Purpose       Provides routing for any path that matches a path managed by the Topics database.  If a match is found, then
|               the user is routed to a template corresponding to the Topic's Content Type.  Otherwise, the originally
|               requested page is rendered (although this may yield a 404).
|
\=============================================================================================================================*/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Routing;
using System.Web.Compilation;
using Ignia.Topics.Configuration;

namespace Ignia.Topics.Web {

  /*==============================================================================================================================
  | CLASS
  \-----------------------------------------------------------------------------------------------------------------------------*/
  public class TopicsRouteHandler : IRouteHandler {

  /*============================================================================================================================
  | PRIVATE MEMBERS
  \---------------------------------------------------------------------------------------------------------------------------*/
    private     List<string>    _views                  = null;

  /*============================================================================================================================
  | CONSTRUCTOR
  \---------------------------------------------------------------------------------------------------------------------------*/
    public TopicsRouteHandler() { }

  /*============================================================================================================================
  | PROPERTY: VIEWS PATH
  >=============================================================================================================================
  | Gets the expected location for View files; attempts to retrieve the value from the <topics /> configuration section, but
  | defaults to ~/Common/Templates/.
  \---------------------------------------------------------------------------------------------------------------------------*/
    private string ViewsPath {
      get {
        string                  viewsPath               = "~/Common/Templates/";
      //Use configuration settings, if available
        TopicsSection           topicsSection           = (TopicsSection)ConfigurationManager.GetSection("topics");
        if (topicsSection != null && topicsSection.Views != null && !String.IsNullOrEmpty(topicsSection.Views.Path)) {
          viewsPath                                     = topicsSection.Views.Path;
        }
        return viewsPath;
      }
    }

  /*============================================================================================================================
  | PROPERTY: VIEWS
  >=============================================================================================================================
  | Iterates through available 'view' template (ASPX) files within the specified directory, collecting them to make them
  | available for validation against a specified View (and consequently, target path).
  \---------------------------------------------------------------------------------------------------------------------------*/
    private List<string> Views {
      get {
        if (_views == null) {

        /*----------------------------------------------------------------------------------------------------------------------
        | DEFINE VIEW TEMPLATE SEARCH VARIABLES
        \---------------------------------------------------------------------------------------------------------------------*/
          List<string>          views                   = new List<string>();
          string                searchPattern           = "*.aspx";
          DirectoryInfo         viewsDirectoryInfo      = new DirectoryInfo(HttpContext.Current.Server.MapPath(ViewsPath));
          SearchOption          searchOption            = SearchOption.TopDirectoryOnly;
          DirectoryInfo[]       subDirectories          = viewsDirectoryInfo.GetDirectories("*", SearchOption.AllDirectories);

        /*----------------------------------------------------------------------------------------------------------------------
        | DISCOVER ALL VIEW TEMPLATES AVAILABLE VIA THE CONFIGURED PATH
        \---------------------------------------------------------------------------------------------------------------------*/
        //Get top-level (generic) view files
          foreach (FileInfo file in viewsDirectoryInfo.GetFiles(searchPattern, searchOption)) {
          //Strip off the extension (must do even for the FileInfo instance)
            string      fileName                        = file.Name.ToLower().Replace(".aspx", "");
            views.Add(fileName);
          }
        //Get view files specific to Content Type
          foreach (DirectoryInfo subDirectory in subDirectories) {
            string      subDirectoryName                = subDirectory.Name;
            foreach (FileInfo file in subDirectory.GetFiles(searchPattern, searchOption)) {
              string    fileName                        = file.Name.ToLower().Replace(".aspx", "");
              views.Add(subDirectoryName + "/" + fileName);
            }
          }

        /*----------------------------------------------------------------------------------------------------------------------
        | SET VIEWS
        \---------------------------------------------------------------------------------------------------------------------*/
          _views                = views;

        }

      /*------------------------------------------------------------------------------------------------------------------------
      | RETURN VIEWS
      \-----------------------------------------------------------------------------------------------------------------------*/
        return _views;

      }
      set {
        _views = value;
      }
    }

  /*============================================================================================================================
  | PROPERTY: IS VALID VIEW
  >-----------------------------------------------------------------------------------------------------------------------------
  | Checks the specified view name against its availability in the Views collection and returns the boolean value of its
  | validity as well as the output variable 'matchedView', indicating the View name.
  \---------------------------------------------------------------------------------------------------------------------------*/
    private bool IsValidView(string contentType, string viewName, out string matchedView) {

    /*--------------------------------------------------------------------------------------------------------------------------
    | CHECK FOR CONTENT TYPE SPECIFIC VIEW
    \-------------------------------------------------------------------------------------------------------------------------*/
      if (
        !String.IsNullOrEmpty(viewName) &&
        Views.Contains(contentType + "/" + viewName, StringComparer.InvariantCultureIgnoreCase) &&
        File.Exists(HttpContext.Current.Server.MapPath(ViewsPath + contentType + "/" + viewName + ".aspx"))
        ) {
        matchedView           = contentType + "/" + viewName;
      }

    /*--------------------------------------------------------------------------------------------------------------------------
    | CHECK FOR GENERIC VIEW
    \-------------------------------------------------------------------------------------------------------------------------*/
      else if (!String.IsNullOrEmpty(viewName) && Views.Contains(viewName, StringComparer.InvariantCultureIgnoreCase)) {
        matchedView             = viewName;
      }

    /*--------------------------------------------------------------------------------------------------------------------------
    | RETURN NULL (INVALID) VIEW
    \-------------------------------------------------------------------------------------------------------------------------*/
      else {
        matchedView             = null;
        return false;
      }
      return true;
    }


  /*============================================================================================================================
  | GET HTTP HANDLER
  >=============================================================================================================================
  | Evaluates the route data to identify the most appropriate HTTP Handler to use, then returns an instance of that handler.
  \---------------------------------------------------------------------------------------------------------------------------*/
    public IHttpHandler GetHttpHandler(RequestContext requestContext) {

    /*--------------------------------------------------------------------------------------------------------------------------
    | SET VARIABLES
    \-------------------------------------------------------------------------------------------------------------------------*/
      string    nameSpace                       = (string)requestContext.RouteData.Values["namespace"]?? "";
      string    path                            = (string)requestContext.RouteData.Values["path"]?? "";
      string    directory                       = nameSpace + "/" + path;

      if (directory.StartsWith("/"))            directory       = directory.Substring(1);
      if (directory.EndsWith("/"))              directory       = directory.Substring(0, directory.Length-1);
      if (path.EndsWith("/"))                   path            = path.Substring(0, path.Length-1);

      Topic     topic                           = TopicRepository.RootTopic.GetTopic(directory.Replace("/", ":"));

    /*--------------------------------------------------------------------------------------------------------------------------
    | VALIDATE PATH
    \-------------------------------------------------------------------------------------------------------------------------*/
      if (topic == null) {
        return BuildManager.CreateInstanceFromVirtualPath("~/" + path, typeof(Page)) as IHttpHandler;
      }

    /*--------------------------------------------------------------------------------------------------------------------------
    | SET ROUTE VARIABLES
    \-------------------------------------------------------------------------------------------------------------------------*/
      string    contentType                     = topic.ContentType.Key;

      if (String.IsNullOrEmpty(nameSpace)) {
        nameSpace                               = topic.UniqueKey.Substring(0, topic.UniqueKey.IndexOf(":"));
        path                                    = topic.UniqueKey.Substring(topic.UniqueKey.IndexOf(":")+1);
      }

      requestContext.RouteData.Values["contentType"]            = contentType;
      requestContext.RouteData.Values["directory"]              = directory;
      requestContext.RouteData.Values["path"]                   = path;

    /*--------------------------------------------------------------------------------------------------------------------------
    | VALIDATE CONTENT TYPE
    \-------------------------------------------------------------------------------------------------------------------------*/
      if (String.IsNullOrEmpty(contentType)) {
        throw new Exception("The ContentType for the Topic \"" + topic.UniqueKey + "\" (" + topic.Id + ") is not set.  Set the ContentType value of the Topic based on the template that should be associated with it.  E.g., a standard page will have the ContentType of \"Page\".");
      }

    /*--------------------------------------------------------------------------------------------------------------------------
    | DERIVE EXPECTED VIEW
    >---------------------------------------------------------------------------------------------------------------------------
    | Check expected page view type and validate against available View (page) templates, based on the following fallback
    | structure: user source (QueryString), Accept header, the Topic's View Attribute (with the default set as the Topic's
    | ContentType Topic View Attribute), and finally to the Content Type name.
    \-------------------------------------------------------------------------------------------------------------------------*/
      string            viewName                = null;

    //Pull from QueryString
      if (viewName == null && HttpContext.Current.Request.QueryString["View"] != null) {
        IsValidView(contentType, HttpContext.Current.Request.QueryString["View"].ToString(), out viewName);
      }

    //Pull from Accept header
      if (viewName == null && HttpContext.Current.Request.Headers["Accept"] != null) {
        string          acceptHeaders           = HttpContext.Current.Request.Headers["Accept"].ToString();
        string[]        splitHeaders            = acceptHeaders.Split(new Char [] {',', ';'});
      //Validate the content-type after the slash, then validate it against available views
        for (int i=0; i < splitHeaders.Length; i++) {
          if (splitHeaders[i].IndexOf("/", StringComparison.InvariantCultureIgnoreCase) >= 0) {
          //Get content-type after the slash and replace '+' characters in the content-type to '-' for view file encoding purposes
            string      acceptHeader            = splitHeaders[i].Substring(splitHeaders[i].IndexOf("/") + 1).Replace("+", "-");
          //Validate against available views; if content-type represents a valid view, stop validation
            if (IsValidView(contentType, acceptHeader, out viewName)) {
              break;
            }
          }
        }
      }

    //Pull from Topic's View Attribute; additional check against the Topic's ContentType Topic View Attribute is not necessary,
    //as it is set as the default View value for the Topic
      if (viewName == null && !String.IsNullOrEmpty(topic.View)) {
        IsValidView(contentType, topic.View, out viewName);
      }

    //Use (fall back to) the Topic's ContentType Attribute
      if (viewName == null) {
        viewName                                = contentType;
      }

    /*--------------------------------------------------------------------------------------------------------------------------
    | SET TARGET PATH
    \-------------------------------------------------------------------------------------------------------------------------*/
      string    targetPath                      = ViewsPath + viewName + ".aspx";

    /*--------------------------------------------------------------------------------------------------------------------------
    | SET TARGET TYPES
    >---------------------------------------------------------------------------------------------------------------------------
    | ###REM JJC101713: Failed attempt to inject strongly typed Topic into page instantiation.  May revisit later.
    >---------------------------------------------------------------------------------------------------------------------------
      Type      topicPageType           = typeof(TypedTopicPage<>);
      Type      topicType               = topic.GetType();
      Type      typedPageType           = topicPageType.MakeGenericType(new Type[] {topicType});
    \-------------------------------------------------------------------------------------------------------------------------*/

    /*--------------------------------------------------------------------------------------------------------------------------
    | RETURN PAGE HANDLER
    \-------------------------------------------------------------------------------------------------------------------------*/
      return BuildManager.CreateInstanceFromVirtualPath(targetPath, typeof(TopicPage)) as IHttpHandler;

    }

  } //Class

} //Namespace