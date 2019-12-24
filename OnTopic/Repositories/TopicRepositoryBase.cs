/*==============================================================================================================================
| Author        Ignia, LLC
| Client        Ignia, LLC
| Project       Topics Library
\=============================================================================================================================*/
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using OnTopic.Internal.Diagnostics;
using OnTopic.Metadata;
using OnTopic.Querying;
using Microsoft;

namespace OnTopic.Repositories {

  /*============================================================================================================================
  | CLASS: TOPIC DATA PROVIDER BASE
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   Defines a base abstract class for taxonomy data providers.
  /// </summary>
  public abstract class TopicRepositoryBase : ITopicRepository {

    /*==========================================================================================================================
    | PRIVATE VARIABLES
    \-------------------------------------------------------------------------------------------------------------------------*/
    private ContentTypeDescriptorCollection? _contentTypeDescriptors = null;

    /*==========================================================================================================================
    | EVENT HANDLERS
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <inheritdoc />
    public event EventHandler<DeleteEventArgs>? DeleteEvent;

    /// <inheritdoc />
    public event EventHandler<MoveEventArgs>? MoveEvent;

    /// <inheritdoc />
    public event EventHandler<RenameEventArgs>? RenameEvent;

    /*==========================================================================================================================
    | GET CONTENT TYPE DESCRIPTORS
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <inheritdoc />
    public virtual ContentTypeDescriptorCollection GetContentTypeDescriptors() {

      /*------------------------------------------------------------------------------------------------------------------------
      | Initialize content types
      \-----------------------------------------------------------------------------------------------------------------------*/
      if (_contentTypeDescriptors == null) {

        /*----------------------------------------------------------------------------------------------------------------------
        | Load configuration data
        \---------------------------------------------------------------------------------------------------------------------*/
        var configuration = Load("Configuration");

        Contract.Assume(configuration, $"The 'Root:Configuration' section could not be loaded from the 'ITopicRepository'.");

        /*----------------------------------------------------------------------------------------------------------------------
        | Add available Content Types to the collection
        \---------------------------------------------------------------------------------------------------------------------*/
        _contentTypeDescriptors = new ContentTypeDescriptorCollection();

        /*----------------------------------------------------------------------------------------------------------------------
        | Ensure the parent ContentTypes topic is available to iterate over
        \---------------------------------------------------------------------------------------------------------------------*/
        var allowedContentTypes = configuration.Children.GetTopic("ContentTypes");

        Contract.Assume(allowedContentTypes, "Unable to load section 'Configuration:ContentTypes'.");

        /*----------------------------------------------------------------------------------------------------------------------
        | Add available Content Types to the collection
        \---------------------------------------------------------------------------------------------------------------------*/
        foreach (var topic in allowedContentTypes.FindAllByAttribute("ContentType", "ContentType")) {
          // Ensure the Topic is used as the strongly-typed ContentType
          // Add ContentType Topic to collection if not already added
          if (
            topic is ContentTypeDescriptor contentTypeDescriptor &&
            !_contentTypeDescriptors.Contains(contentTypeDescriptor.Key)
          ) {
            _contentTypeDescriptors.Add(contentTypeDescriptor);
          }
        }

      }

      return _contentTypeDescriptors;

    }

    /*==========================================================================================================================
    | METHOD: LOAD
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <inheritdoc />
    public abstract Topic? Load(int topicId, bool isRecursive = true);

    /// <inheritdoc />
    public abstract Topic? Load(string? topicKey = null, bool isRecursive = true);

    /// <inheritdoc />
    public abstract Topic? Load(int topicId, DateTime version);

    /*==========================================================================================================================
    | METHOD: ROLLBACK
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <inheritdoc />
    public virtual void Rollback([ValidatedNotNull, NotNull]Topic topic, DateTime version) {

      /*------------------------------------------------------------------------------------------------------------------------
      | Validate parameters
      \-----------------------------------------------------------------------------------------------------------------------*/
      Contract.Requires(topic, nameof(topic));
      Contract.Requires(version, nameof(version));
      Contract.Requires<ArgumentException>(
        topic.VersionHistory.Contains(version),
        "The version requested for rollback does not exist in the version history"
      );

      /*------------------------------------------------------------------------------------------------------------------------
      | Retrieve topic from database
      \-----------------------------------------------------------------------------------------------------------------------*/
      var originalVersion = Load(topic.Id, version);

      Contract.Assume(
        originalVersion,
        "The version requested for rollback does not exist in the Topic repository or database."
      );

      /*------------------------------------------------------------------------------------------------------------------------
      | Mark each attribute as dirty
      \-----------------------------------------------------------------------------------------------------------------------*/
      foreach (var attribute in originalVersion.Attributes) {
        if (!topic.Attributes.Contains(attribute.Key) || topic.Attributes.GetValue(attribute.Key) != attribute.Value) {
          attribute.IsDirty = true;
        }
      }

      /*------------------------------------------------------------------------------------------------------------------------
      | Construct new AttributeCollection
      \-----------------------------------------------------------------------------------------------------------------------*/
      topic.Attributes.Clear();
      foreach (var attribute in originalVersion.Attributes) {
        topic.Attributes.Add(attribute);
      }

      /*------------------------------------------------------------------------------------------------------------------------
      | Rename topic, if necessary
      \-----------------------------------------------------------------------------------------------------------------------*/
      if (topic.Key == originalVersion.Key) {
        topic.Attributes.SetValue("Key", topic.Key, false);
      }
      else {
        topic.Key = originalVersion.Key;
      }

      /*------------------------------------------------------------------------------------------------------------------------
      | Ensure Parent, ContentType are maintained
      \-----------------------------------------------------------------------------------------------------------------------*/
      topic.Attributes.SetValue("ContentType", topic.ContentType, topic.ContentType != originalVersion.ContentType);
      topic.Attributes.SetValue("ParentId", topic.Parent?.Id.ToString(CultureInfo.InvariantCulture)?? "-1", false);

      /*------------------------------------------------------------------------------------------------------------------------
      | Save as new version
      \-----------------------------------------------------------------------------------------------------------------------*/
      Save(topic, false);

    }

    /*==========================================================================================================================
    | METHOD: SAVE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <inheritdoc />
    public virtual int Save([ValidatedNotNull, NotNull]Topic topic, bool isRecursive = false, bool isDraft = false) {

      /*------------------------------------------------------------------------------------------------------------------------
      | Validate parameters
      \-----------------------------------------------------------------------------------------------------------------------*/
      Contract.Requires(topic, nameof(topic));

      /*------------------------------------------------------------------------------------------------------------------------
      | Validate content type
      \-----------------------------------------------------------------------------------------------------------------------*/
      _contentTypeDescriptors = GetContentTypeDescriptors();
      if (!_contentTypeDescriptors.Contains(topic.ContentType)) {
        throw new ArgumentException(
          $"The Content Type \"{topic.ContentType}\" referenced by \"{topic.Key}\" could not be found under " +
          $"\"Configuration:ContentTypes\". There are currently {_contentTypeDescriptors.Count} ContentTypes in the Repository."
        );
      }

      /*------------------------------------------------------------------------------------------------------------------------
      | Update content types collection, if appropriate
      \-----------------------------------------------------------------------------------------------------------------------*/
      if (topic is ContentTypeDescriptor && !_contentTypeDescriptors.Contains(topic.Key)) {
        _contentTypeDescriptors.Add((ContentTypeDescriptor)topic);
      }

      /*------------------------------------------------------------------------------------------------------------------------
      | Trigger event
      \-----------------------------------------------------------------------------------------------------------------------*/
      if (topic.OriginalKey != null && topic.OriginalKey != topic.Key) {
        var args = new RenameEventArgs(topic);
        RenameEvent?.Invoke(this, args);
      }

      /*----------------------------------------------------------------------------------------------------------------------
      | Perform reordering and/or move
      \---------------------------------------------------------------------------------------------------------------------*/
      if (topic.Parent != null && topic.Attributes.IsDirty("ParentId") && topic.Id >= 0) {
        var topicIndex = topic.Parent.Children.IndexOf(topic);
        if (topicIndex > 0) {
          Move(topic, topic.Parent, topic.Parent.Children[topicIndex - 1]);
        }
        else {
          Move(topic, topic.Parent);
        }
      }

      /*------------------------------------------------------------------------------------------------------------------------
      | Reset original key
      \-----------------------------------------------------------------------------------------------------------------------*/
      topic.OriginalKey = null;
      return -1;

    }

    /*==========================================================================================================================
    | METHOD: MOVE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <inheritdoc />
    public virtual void Move([ValidatedNotNull, NotNull]Topic topic, [ValidatedNotNull, NotNull]Topic target) {

      /*------------------------------------------------------------------------------------------------------------------------
      | Validate parameters
      \-----------------------------------------------------------------------------------------------------------------------*/
      Contract.Requires(target != topic);
      Contract.Requires(topic, "The topic parameter must be specified.");
      Contract.Requires(target, "The target parameter must be specified.");
      if (topic.Parent != target) {
        MoveEvent?.Invoke(this, new MoveEventArgs(topic, target));
        topic.SetParent(target);
      }

    }

    /// <summary>
    ///   Interface method that supports moving a topic from one position to another.
    /// </summary>
    /// <param name="topic">The topic object to be moved.</param>
    /// <param name="target">A topic object under which to move the source topic.</param>
    /// <param name="sibling">A topic object representing a sibling adjacent to which the topic should be moved.</param>
    /// <returns>Boolean value representing whether the operation completed successfully.</returns>
    public virtual void Move([ValidatedNotNull, NotNull]Topic topic, [ValidatedNotNull, NotNull]Topic target, Topic? sibling) {

      /*------------------------------------------------------------------------------------------------------------------------
      | Validate parameters
      \-----------------------------------------------------------------------------------------------------------------------*/
      Contract.Requires(target != topic);
      Contract.Requires(topic, nameof(topic));
      Contract.Requires(target, nameof(target));
      Contract.Requires<ArgumentException>(topic != target, "A topic cannot be its own parent.");
      Contract.Requires<ArgumentException>(topic != sibling, "A topic cannot be moved relative to itself.");

      /*------------------------------------------------------------------------------------------------------------------------
      | Ignore requests
      \-----------------------------------------------------------------------------------------------------------------------*/
      //If the target is already positioned after the sibling, then no actual change is registered
      if (
        sibling != null &&
        topic.Parent != null &&
        topic.Parent.Children.IndexOf(sibling) == topic.Parent.Children.IndexOf(topic)-1) {
        return;
      }

      /*------------------------------------------------------------------------------------------------------------------------
      | Perform base logic
      \-----------------------------------------------------------------------------------------------------------------------*/
      MoveEvent?.Invoke(this, new MoveEventArgs(topic, target));
      topic.SetParent(target, sibling);

    }

    /*==========================================================================================================================
    | METHOD: DELETE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <inheritdoc />
    public virtual void Delete([ValidatedNotNull, NotNull]Topic topic, bool isRecursive) {

      /*------------------------------------------------------------------------------------------------------------------------
      | Validate parameters
      \-----------------------------------------------------------------------------------------------------------------------*/
      Contract.Requires(topic, nameof(topic));

      /*------------------------------------------------------------------------------------------------------------------------
      | Trigger event
      \-----------------------------------------------------------------------------------------------------------------------*/
      var         args    = new DeleteEventArgs(topic);
      DeleteEvent?.Invoke(this, args);

      /*------------------------------------------------------------------------------------------------------------------------
      | Remove from parent
      \-----------------------------------------------------------------------------------------------------------------------*/
      if (topic.Parent != null) {
        topic.Parent.Children.Remove(topic.Key);
      }

    }

  } //Class
} //Namespace