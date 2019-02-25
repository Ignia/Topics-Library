﻿/*==============================================================================================================================
| Author        Ignia, LLC
| Client        Ignia, LLC
| Project       Topics Library
\=============================================================================================================================*/
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ignia.Topics.Internal.Mapping;
using Ignia.Topics.Internal.Reflection;
using Ignia.Topics.Metadata;
using Ignia.Topics.Models;
using Ignia.Topics.Reflection;
using Ignia.Topics.Repositories;

namespace Ignia.Topics.Mapping {

  /*============================================================================================================================
  | CLASS: BINDING MODEL VALIDATOR
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   The model validator is responsible for identifying issues with tyopic view or binding models by evaluating their types
  ///   against a <see cref="ContentTypeDescriptor"/>
  /// </summary>
  /// <remarks>
  ///   <para>
  ///     Technically, there is no reason for this code not to be in <see cref="ReverseTopicMappingService"/>, which it is
  ///     specific to. The main reason it is separated out into a separate internal class is due to the size. The length is not
  ///     due to the code being especially complicated; it just has detailed documentation and exception messages. If this were
  ///     intended to be used by other classes, it'd be configured as an injectible public dependency; as an internal helper
  ///     class, however, it is adequate as a set of static methods.
  ///   </para>
  ///   <para>
  ///     There may be value to providing a comparable class for <see cref="TopicMappingService"/> in the future. That said, it
  ///     is both easier to validate binding models (since they rely on interfaces, and aren't POCOs) and more critical (since
  ///     errors in mapping don't just result in missing data in the user interface, but potentially corrupt the database). As a
  ///     result, this class is provided to help developers avoid coding errors by validating design time decisions (the
  ///     binding model) against runtime behavior (the type of content type it is being utilized against).
  ///   </para>
  /// </remarks>
  static internal class BindingModelValidator {

    /*==========================================================================================================================
    | PRIVATE FIELDS
    \-------------------------------------------------------------------------------------------------------------------------*/
    static readonly ConcurrentBag<(Type, string)> _modelsValidated = new ConcurrentBag<(Type, string)>();

    /*==========================================================================================================================
    | PROTECTED: VALIDATE MODEL
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Helper function that evaluates the binding model against the associated content type to identify any potential
    ///   mapping errors relative to the schema.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     This helper function is intended to provide reporting to developers about errors in their model. As a result, it
    ///     will exclusively throw exceptions, as opposed to populating validation object for rendering to the view. Because
    ///     it's only evaluating the compiled model, which will not change during the application's life cycle, the <paramref
    ///     name="sourceType"/> and <paramref name="contentTypeDescriptor"/> are stored as a <see cref="Tuple"/> in a static
    ///     <see cref="ConcurrentBag{T}"/> once a particular combination has passed validation—that way, this check only needs
    ///     to be executed once for any combination, at least for the current application life cycle.
    ///   </para>
    ///   <para>
    ///     Nested <see cref="Type"/>s from e.g. Nested Topics, relationships, and topic references will not be evaluated until
    ///     they are called from <see cref="IReverseTopicMappingService.MapAsync(ITopicBindingModel)"/> (or one if its
    ///     overloads). This could be problematic since any downstream errors could leave the <see cref="ITopicRepository"/>
    ///     in an invalid state. That said, since this is identifying what are effectively bugs in the <see
    ///     cref="ITopicBindingModel"/> implementation, it is hoped that these will be identified during development, and never
    ///     make it to a production environment. To be safe, however, developers should be cautious when running mapping
    ///     existing <see cref="Topic"/> instances from the <see cref="ITopicRepository"/> when the <see
    ///     cref="ContentTypeDescriptor"/> may not have undergone explicit testing.
    ///   </para>
    /// </remarks>
    /// <param name="sourceType">
    ///   The binding model <see cref="Type"/> to validate.
    /// </param>
    /// <param name="properties">
    ///   A <see cref="MemberInfoCollection{PropertyInfo}"/> describing the <paramref name="sourceType"/>'s properties.
    /// </param>
    /// <param name="contentTypeDescriptor">
    ///   The <see cref="ContentTypeDescriptor"/> object against which to validate the model.
    /// </param>
    static internal void ValidateModel(
      Type sourceType,
      MemberInfoCollection<PropertyInfo> properties,
      ContentTypeDescriptor contentTypeDescriptor
      ) {

      /*------------------------------------------------------------------------------------------------------------------------
      | Skip validation if this type has already been validated for this content type
      \-----------------------------------------------------------------------------------------------------------------------*/
      if (_modelsValidated.Contains((sourceType, contentTypeDescriptor.Key))) {
        return;
      }

      /*------------------------------------------------------------------------------------------------------------------------
      | Validate
      \-----------------------------------------------------------------------------------------------------------------------*/
      foreach (var property in properties) {
        ValidateProperty(sourceType, property, contentTypeDescriptor);
      }

      /*------------------------------------------------------------------------------------------------------------------------
      | Add type, content type to model validation cache so it isn't checked again
      \-----------------------------------------------------------------------------------------------------------------------*/
      _modelsValidated.Add((sourceType, contentTypeDescriptor.Key));

      return;

    }

    /*==========================================================================================================================
    | PROTECTED: VALIDATE PROPERTY
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Helper function that evaluates a property of the binding model against the associated content type to identify any
    ///   potential mapping errors relative to the schema.
    /// </summary>
    /// <param name="sourceType">
    ///   The binding model <see cref="Type"/> to validate.
    /// </param>
    /// <param name="property">
    ///   A <see cref="PropertyInfo"/> describing a specific property of the <paramref name="sourceType"/>.
    /// </param>
    /// <param name="contentTypeDescriptor">
    ///   The <see cref="ContentTypeDescriptor"/> object against which to validate the model.
    /// </param>
    static internal void ValidateProperty(
      Type sourceType,
      PropertyInfo property,
      ContentTypeDescriptor contentTypeDescriptor
    ) {

      /*------------------------------------------------------------------------------------------------------------------------
      | Define variables
      \-----------------------------------------------------------------------------------------------------------------------*/
      var configuration         = new PropertyConfiguration(property);
      var attributeDescriptor   = contentTypeDescriptor.AttributeDescriptors.GetTopic(configuration.AttributeKey);
      var childRelationships    = new[] { RelationshipType.Children, RelationshipType.NestedTopics };
      var relationships         = new[] { RelationshipType.Relationship, RelationshipType.IncomingRelationship };
      var listType              = (Type)null;

      /*------------------------------------------------------------------------------------------------------------------------
      | Define list type (if it's a list and it's generic)
      \-----------------------------------------------------------------------------------------------------------------------*/
      if (typeof(IList).IsAssignableFrom(property.PropertyType) && configuration.Property.PropertyType.IsGenericType) {
        listType = configuration.Property.PropertyType.GetGenericArguments().Last();
      }

      /*------------------------------------------------------------------------------------------------------------------------
      | Handle children
      \-----------------------------------------------------------------------------------------------------------------------*/
      if (configuration.RelationshipType == RelationshipType.Children) {
        throw new InvalidOperationException(
          $"The {nameof(ReverseTopicMappingService)} does not support mapping child topics. This property should be " +
          $"removed from the binding model, or otherwise decorated with the {nameof(DisableMappingAttribute)} to prevent " +
          $"it from being evaluated by the {nameof(ReverseTopicMappingService)}. If children must be mapped, then the " +
          $"caller should handle this on a per child basis, where it can better validate the merge logic given the current " +
          $"context of the target topic."
        );
      }

      /*------------------------------------------------------------------------------------------------------------------------
      | Handle parent
      \-----------------------------------------------------------------------------------------------------------------------*/
      if (configuration.AttributeKey == "Parent") {
        throw new InvalidOperationException(
          $"The {nameof(ReverseTopicMappingService)} does not support mapping Parent topics. This property should be " +
          $"removed from the binding model, or otherwise decorated with the {nameof(DisableMappingAttribute)} to prevent " +
          $"it from being evaluated by the {nameof(ReverseTopicMappingService)}."
        );
      }

      /*------------------------------------------------------------------------------------------------------------------------
      | Validate attribute type
      \-----------------------------------------------------------------------------------------------------------------------*/
      if (attributeDescriptor == null) {
        throw new InvalidOperationException(
          $"A {nameof(sourceType)} object was provided with a content type set to {contentTypeDescriptor.Key}'. This " +
          $"content type does not contain an attribute named '{configuration.AttributeKey}', as requested by the " +
          $"{configuration.Property.Name} property. If this property is not intended to be mapped by the " +
          $"{nameof(ReverseTopicMappingService)}, then it should be decorated with {nameof(DisableMappingAttribute)}."
        );
      }

      /*------------------------------------------------------------------------------------------------------------------------
      | Detect non-mapped relationships
      \-----------------------------------------------------------------------------------------------------------------------*/
      if (typeof(IList).IsAssignableFrom(property.PropertyType) && configuration.RelationshipType == RelationshipType.Any) {
        throw new InvalidOperationException(
          $"The {property.Name} on the {sourceType.Name} is a collection, but it is ambiguous what relationship on the " +
          $"{nameof(ContentTypeDescriptor)} that is represents. If it is intended to be mapped, include the " +
          $"{nameof(RelationshipAttribute)}. Otherwise, include the {nameof(DisableMappingAttribute)} to indicate that this" +
          $"property is not intended to be mapped."
        );
      }

      /*------------------------------------------------------------------------------------------------------------------------
      | Validate the correct base class for relationships
      \-----------------------------------------------------------------------------------------------------------------------*/
      if (
        relationships.Contains(configuration.RelationshipType) &&
        !typeof(IRelatedTopicBindingModel).IsAssignableFrom(listType)
      ) {
        throw new InvalidOperationException(
          $"The {property.Name} on the {sourceType.Name} has been determined to be a {configuration.RelationshipType}, but " +
          $"the generic type {listType.Name} does not implement the {typeof(IRelatedTopicBindingModel)} interface. This is " +
          $"required for binding models. If this collection is not intended to be mapped to " +
          $"{configuration.RelationshipType} then use the {nameof(RelationshipAttribute)} to map it to a different " +
          $"{nameof(RelationshipType)}. If this collection is not intended to be mapped at all, include the " +
          $"{nameof(DisableMappingAttribute)} to exclude it from mapping."
        );
      }

      /*------------------------------------------------------------------------------------------------------------------------
      | Validate the correct base class for children
      \-----------------------------------------------------------------------------------------------------------------------*/
      if (
        childRelationships.Contains(configuration.RelationshipType) &&
        !typeof(ITopicBindingModel).IsAssignableFrom(listType)
      ) {
        throw new InvalidOperationException(
          $"The {property.Name} on the {sourceType.Name} has been determined to be a {configuration.RelationshipType}, but " +
          $"the generic type {listType.Name} does not implement the {typeof(ITopicBindingModel)} interface. This is " +
          $"required for binding models. If this collection is not intended to be mapped to " +
          $"{configuration.RelationshipType} then use the {nameof(RelationshipAttribute)} to map it to a different " +
          $"{nameof(RelationshipType)}. If this collection is not intended to be mapped at all, include the " +
          $"{nameof(DisableMappingAttribute)} to exclude it from mapping."
        );
      }

      /*------------------------------------------------------------------------------------------------------------------------
      | Validate that references end in "Id"
      \-----------------------------------------------------------------------------------------------------------------------*/
      if (
        !typeof(ITopicBindingModel).IsAssignableFrom(property.PropertyType) &&
        !configuration.AttributeKey.EndsWith("Id", StringComparison.InvariantCulture)
      ) {
        throw new InvalidOperationException(
          $"The {property.Name} on the {sourceType.Name} has been determined to be a topic reference, but the generic type " +
          $"{configuration.AttributeKey} does not end in <c>Id</c>. By convention, all topic reference are expected to end " +
          $"in <c>Id</c>. To keep the property name set to {property.PropertyType.Name}, use the " +
          $"{nameof(AttributeKeyAttribute)} to specify the name of the topic reference this should map to. If this " +
          $"property is not intended to be mapped at all, include the {nameof(DisableMappingAttribute)}. If the " +
          $"{contentTypeDescriptor.Key} defines a topic reference attribute that doesn't follow this convention, then it " +
          $"should be updated."
        );
      }

    }

  } //Class
} //Namespace