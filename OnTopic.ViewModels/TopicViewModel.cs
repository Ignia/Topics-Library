﻿/*==============================================================================================================================
| Author        Ignia, LLC
| Client        Ignia, LLC
| Project       Topics Library
\=============================================================================================================================*/
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using OnTopic.Mapping.Annotations;
using OnTopic.Models;

namespace OnTopic.ViewModels {

  /*============================================================================================================================
  | VIEW MODEL: TOPIC
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   Provides a model for feeding views general information about a <see cref="Topic"/>.
  /// </summary>
  /// <remarks>
  ///   Typically, view models should be created as part of the presentation layer. The <see cref="Models"/> namespace contains
  ///   default implementations that can be used directly, used as base classes, or overwritten at the presentation level. They
  ///   are supplied for convenience to model factory default settings for out-of-the-box content types.
  /// </remarks>
  public record TopicViewModel: ITopicViewModel, ICoreTopicViewModel, IAssociatedTopicBindingModel, ITopicBindingModel {

    /*==========================================================================================================================
    | ID
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <inheritdoc />
    public int Id { get; init; }

    /*==========================================================================================================================
    | KEY
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <inheritdoc />
    [Required]
    public string Key { get; init; } = default!;

    /*==========================================================================================================================
    | CONTENT TYPE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <inheritdoc />
    [Required]
    public string ContentType { get; init; } = default!;

    /*==========================================================================================================================
    | UNIQUE KEY
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <inheritdoc />
    [Required]
    public string UniqueKey { get; init; } = default!;

    /*==========================================================================================================================
    | WEB PATH
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <inheritdoc />
    [Required]
    public string WebPath { get; init; } = default!;

    /*==========================================================================================================================
    | VIEW
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <inheritdoc />
    public string? View { get; init; }

    /*==========================================================================================================================
    | TITLE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <inheritdoc />
    [Required]
    public string Title { get; init; } = default!;

    /*==========================================================================================================================
    | IS HIDDEN?
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <inheritdoc/>
    [ExcludeFromCodeCoverage]
    [Obsolete("The IsHidden property is no longer supported by TopicViewModel.", true)]
    [DisableMapping]
    public bool IsHidden { get; init; }

    /*==========================================================================================================================
    | LAST MODIFIED
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   The date that the topic was last modified on.
    /// </summary>
    public DateTime LastModified { get; init; }

    /*==========================================================================================================================
    | PARENT
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Provides a reference to the parent <see cref="TopicViewModel"/> in the topic hierarchy.
    /// </summary>
    /// <remarks>
    ///   If the current <see cref="TopicViewModel"/> is being mapped as part of another <see cref="TopicViewModel"/>, then the
    ///   <see cref="Parent"/> property will only be mapped if that association includes a <see cref="IncludeAttribute"/> with a
    ///   value including <see cref="AssociationTypes.Parents"/>. If it does, all <see cref="Parent"/> topics will be mapped up
    ///   to the root of the site. No other associations on the <see cref="Parent"/> view models will be mapped, even if they
    ///   are annotated with a <see cref="IncludeAttribute"/>.
    /// </remarks>
    [Include(AssociationTypes.Parents)]
    public TopicViewModel? Parent { get; init; }

  } //Class
} //Namespace