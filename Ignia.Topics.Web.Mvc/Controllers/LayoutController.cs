﻿/*==============================================================================================================================
| Author        Ignia, LLC
| Client        Ignia, LLC
| Project       Topics Library
\=============================================================================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ignia.Topics;
using Ignia.Topics.Mapping;
using Ignia.Topics.Repositories;
using Ignia.Topics.Web.Mvc.Models;

namespace Ignia.Topics.Web.Mvc.Controllers {

  /*============================================================================================================================
  | CLASS: LAYOUT CONTROLLER
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   Provides access to views for populating specific layout dependencies, such as the <see cref="Menu"/>.
  /// </summary>
  /// <remarks>
  ///   As a best practice, global data required by the layout view are requested independently of the current page. This allows
  ///   each layout element to be provided with its own layout data, in the form of <see cref="NavigationViewModel{T}"/>s,
  ///   instead of needing to add this data to every view model returned by <see cref="TopicController"/>. The <see
  ///   cref="LayoutController{T}"/> facilitates this by not only providing a default implementation for <see cref="Menu"/>, but
  ///   additionally providing protected helper methods that aid in locating and assembling <see cref="Topic"/> and <see
  ///   cref="INavigationTopicViewModelCore"/> references that are relevant to specific layout elements.
  /// </remarks>
  public class LayoutController<T> : Controller where T : class, INavigationTopicViewModelCore, new() {

    /*==========================================================================================================================
    | PRIVATE VARIABLES
    \-------------------------------------------------------------------------------------------------------------------------*/
    private readonly            ITopicRepository                _topicRepository                = null;
    private readonly            ITopicRoutingService            _topicRoutingService            = null;
    private readonly            ITopicMappingService            _topicMappingService            = null;
    private                     Topic                           _currentTopic                   = null;

    /*==========================================================================================================================
    | CONSTRUCTOR
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Initializes a new instance of a Topic Controller with necessary dependencies.
    /// </summary>
    /// <returns>A topic controller for loading OnTopic views.</returns>
    public LayoutController(
      ITopicRepository topicRepository,
      ITopicRoutingService topicRoutingService,
      ITopicMappingService topicMappingService
    ) : base() {
      _topicRepository          = topicRepository;
      _topicRoutingService      = topicRoutingService;
      _topicMappingService      = topicMappingService;
    }

    /*==========================================================================================================================
    | TOPIC REPOSITORY
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Provides a reference to the Topic Repository in order to gain arbitrary access to the entire topic graph.
    /// </summary>
    /// <returns>The TopicRepository associated with the controller.</returns>
    protected ITopicRepository TopicRepository {
      get {
        return _topicRepository;
      }
    }

    /*==========================================================================================================================
    | CURRENT TOPIC
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Provides a reference to the current topic associated with the request.
    /// </summary>
    /// <returns>The Topic associated with the current request.</returns>
    protected Topic CurrentTopic {
      get {
        if (_currentTopic == null) {
          _currentTopic = _topicRoutingService.GetCurrentTopic();
        }
        return _currentTopic;
      }
    }

    /*==========================================================================================================================
    | MENU
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Provides the global menu for the site layout, which exposes the top two tiers of navigation.
    /// </summary>
    public virtual PartialViewResult Menu() {

      /*------------------------------------------------------------------------------------------------------------------------
      | Establish variables
      \-----------------------------------------------------------------------------------------------------------------------*/
      var currentTopic          = CurrentTopic;
      var navigationRootTopic   = (Topic)null;

      /*------------------------------------------------------------------------------------------------------------------------
      | Identify navigation root
      >-------------------------------------------------------------------------------------------------------------------------
      | The navigation root in the case of the main menu is the namespace; i.e., the first topic underneath the root.
      \-----------------------------------------------------------------------------------------------------------------------*/
      navigationRootTopic = GetNavigationRoot(currentTopic, 2, "Web");

      /*------------------------------------------------------------------------------------------------------------------------
      | Construct view model
      \-----------------------------------------------------------------------------------------------------------------------*/
      var navigationViewModel   = new NavigationViewModel<T>() {
        NavigationRoot          = AddNestedTopics(navigationRootTopic, false, 3),
        CurrentKey              = CurrentTopic?.GetUniqueKey()
      };

      /*------------------------------------------------------------------------------------------------------------------------
      | Return the corresponding view
      \-----------------------------------------------------------------------------------------------------------------------*/
      return PartialView(navigationViewModel);

    }

    /*==========================================================================================================================
    | GET NAVIGATION ROOT
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   A helper function that will crawl up the current tree and retrieve the topic that is <paramref name="fromRoot"/> tiers
    ///   down from the root of the topic graph.
    /// </summary>
    /// <remarks>
    ///   Often, an action of a <see cref="LayoutController{T}"/> will need a reference to a topic at a certain level, which
    ///   represents the navigation for the site. For instance, if the primary navigation is at <c>Root:Web</c>, then the
    ///   navigation is one level from the root (i.e., <paramref name="fromRoot"/>=1). This, however, should not be hard-coded
    ///   in case a site has multiple roots. For instance, if a page is under <c>Root:Library</c> then <i>that</i> should be the
    ///   navigation root. This method provides support for these scenarios.
    /// </remarks>
    /// <param name="currentTopic">The <see cref="Topic"/> to start from.</param>
    /// <param name="fromRoot">The distance that the navigation root should be from the root of the topic graph.</param>
    /// <param name="defaultRoot">If a root cannot be identified, the default root that should be returned.</param>
    protected Topic GetNavigationRoot(Topic currentTopic, int fromRoot = 2, string defaultRoot = "Web") {
      var navigationRootTopic = currentTopic;
      while (DistanceFromRoot(navigationRootTopic) > fromRoot) {
        navigationRootTopic = navigationRootTopic.Parent;
      }

      if (navigationRootTopic == null && !String.IsNullOrWhiteSpace(defaultRoot)) {
        navigationRootTopic = TopicRepository.Load(defaultRoot);
      }

      return navigationRootTopic;

    }

    /*==========================================================================================================================
    | DISTANCE FROM ROOT
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   A helper function that will determine how far a given topic is from the root of a tree.
    /// </summary>
    /// <param name="sourceTopic">The <see cref="Topic"/> to pull the values from.</param>
    private int DistanceFromRoot(Topic sourceTopic) {
      var distance = 1;
      while (sourceTopic.Parent != null) {
        sourceTopic = sourceTopic.Parent;
        distance++;
      }
      return distance;
    }

    /*==========================================================================================================================
    | ADD NESTED TOPICS
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   A helper function that allows a set number of tiers to be added to a <see cref="NavigationViewModel"/> tree.
    /// </summary>
    /// <param name="sourceTopic">The <see cref="Topic"/> to pull the values from.</param>
    /// <param name="allowPageGroups">Determines whether <see cref="PageGroupTopicViewModel"/>s should be crawled.</param>
    /// <param name="tiers">Determines how many tiers of children should be included in the graph.</param>
    protected T AddNestedTopics(
      Topic sourceTopic,
      bool allowPageGroups      = true,
      int tiers                 = 1
    ) {
      tiers--;
      if (sourceTopic == null) {
        return null as T;
      }
      var viewModel = _topicMappingService.Map<T>(sourceTopic, Relationships.None);
      if (tiers >= 0 && (allowPageGroups || !sourceTopic.ContentType.Equals("PageGroup"))) {
        foreach (var topic in sourceTopic.Children.Sorted.Where(t => t.IsVisible())) {
          viewModel.Children.Add(
            AddNestedTopics(
              topic,
              allowPageGroups,
              tiers
            )
          );
        }
      }
      return viewModel;
    }

  } // Class

} // Namespace