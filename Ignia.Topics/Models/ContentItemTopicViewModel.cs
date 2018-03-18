﻿/*==============================================================================================================================
| Author        Ignia, LLC
| Client        Ignia, LLC
| Project       Topics Library
\=============================================================================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignia.Topics.Models {

  /*============================================================================================================================
  | VIEW MODEL: CONTENT ITEM TOPIC
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   Provides a strongly-typed data transfer object for feeding views with information about a content item topic.
  /// </summary>
  /// <remarks>
  ///   Typically, view models should be created as part of the presentation layer. The <see cref="Models"/> namespace contains
  ///   default implementations that can be used directly, used as base classes, or overwritten at the presentative level. They
  ///   are supplied for convenience to model factory default settings for out-of-the-box content types.
  /// </remarks>
  public class ContentItemTopicViewModel: ItemTopicViewModel {

    public string LearnMoreUrl { get; set; }
    public string ThumbnailImage { get; set; }
    public string Category { get; set; }

  } //Class
} //Namespace
