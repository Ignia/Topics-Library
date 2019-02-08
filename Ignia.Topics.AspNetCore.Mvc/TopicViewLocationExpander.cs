﻿/*==============================================================================================================================
| Author        Ignia, LLC
| Client        Ignia, LLC
| Project       Topics Library
\=============================================================================================================================*/
using System;
using System.Linq;
using System.Collections.Generic;
using Ignia.Topics.Diagnostics;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Ignia.Topics.AspNetCore.Mvc {

  /*============================================================================================================================
  | CLASS: TOPIC VIEW LOCATION EXPANDER
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   Provides hints to the configured view engine (presumably, <see cref="RazorViewEngine"/>) on where to find views
  ///   associated with the current request.
  /// </summary>
  /// <remarks>
  ///   In addition to Area (<c>{2}</c>), Controller (<c>{1}</c>), and View (<c>{0}</c>), the <see
  ///   cref="TopicViewLocationExpander"/> also factors in ContentType (<c>{3}</c>).
  /// </remarks>
  public class TopicViewLocationExpander : IViewLocationExpander {

    /*==========================================================================================================================
    | PRIVATE VARIABLES
    \-------------------------------------------------------------------------------------------------------------------------*/

    /*==========================================================================================================================
    | CONSTRUCTOR
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Initializes a new instance of the <see cref="TopicViewLocationExpander"/> class.
    /// </summary>
    public TopicViewLocationExpander() {
    }

    /*==========================================================================================================================
    | PROPERTY: VIEW LOCATIONS
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Retrieves a static copy of all view locations associated with OnTopic.
    /// </summary>
    public static IEnumerable<string> ViewLocations => new[] {
      "~/Views/{3}/{1}.cshtml",
      "~/Views/ContentTypes/{3}.{1}.cshtml",
      "~/Views/ContentTypes/{1}.cshtml",
      "~/Views/Shared/{1}.cshtml",
    };

    /*==========================================================================================================================
    | PROPERTY: AREA VIEW LOCATIONS
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Retrieves a static copy of all areas view locations associated with OnTopic.
    /// </summary>
    public static IEnumerable<string> AreaViewLocations => new[] {
      "~/{2}/Views/{3}/{1}.cshtml",
      "~/{2}/Views/ContentTypes/{3}.{1}.cshtml",
      "~/{2}/Views/ContentTypes/{1}.cshtml",
      "~/{2}/Views/Shared/{1}.cshtml",
    };

    /*==========================================================================================================================
    | METHOD: POPULATE VALUES
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Initializes a new instance of the <see cref="TopicViewLocationExpander"/> class.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <seealso cref="https://stackoverflow.com/questions/36802661/what-is-iviewlocationexpander-populatevalues-for-in-asp-net-core-mvc"/>
    /// <param name="context">The <see cref="ViewLocationExpanderContext"/> that the request is operating within.</param>
    public void PopulateValues(ViewLocationExpanderContext context) {
      context.Values["action_displayname"] = context.ActionContext.ActionDescriptor.DisplayName;
    }

    /*==========================================================================================================================
    | METHOD: EXPAND VIEW LOCATIONS
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Introduces additional routes
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <param name="context">The <see cref="ViewLocationExpanderContext"/> that the request is operating within.</param>
    public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations) {

      /*------------------------------------------------------------------------------------------------------------------------
      | Validate parameters
      \-----------------------------------------------------------------------------------------------------------------------*/
      Contract.Requires<ArgumentNullException>(context != null, nameof(context));
      Contract.Requires<ArgumentNullException>(viewLocations != null, nameof(viewLocations));

      /*------------------------------------------------------------------------------------------------------------------------
      | Establish variables
      \-----------------------------------------------------------------------------------------------------------------------*/
      var controllerDescriptor  = context.ActionContext.ActionDescriptor as ControllerActionDescriptor;

      controllerDescriptor.RouteValues.TryGetValue("contenttype", out var contentType);

      /*------------------------------------------------------------------------------------------------------------------------
      | Yield view locations
      \-----------------------------------------------------------------------------------------------------------------------*/
      foreach (var location in ViewLocations) {
        yield return location.Replace(@"{3}", contentType);
      }

      /*------------------------------------------------------------------------------------------------------------------------
      | Yield are view locations
      \-----------------------------------------------------------------------------------------------------------------------*/
      foreach (var location in AreaViewLocations) {
        yield return location.Replace(@"{3}", contentType);
      }

      /*------------------------------------------------------------------------------------------------------------------------
      | Return previous
      \-----------------------------------------------------------------------------------------------------------------------*/
      foreach (var location in viewLocations) {
        yield return location;
      }

    }

  } //Class
} //Namespace
