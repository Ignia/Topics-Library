﻿/*==============================================================================================================================
| Author        Ignia, LLC
| Client        Ignia, LLC
| Project       Topics Library
\=============================================================================================================================*/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ignia.Topics.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Ignia.Topics.Reflection {

  /*============================================================================================================================
  | CLASS: TYPE COLLECTION
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   A collection of <see cref="MemberInfoCollection"/> instances, each associated with a specific <see cref="Type"/>.
  /// </summary>
  internal class TypeMemberInfoCollection : KeyedCollection<Type, MemberInfoCollection> {

    /*==========================================================================================================================
    | PRIVATE VARIABLES
    \-------------------------------------------------------------------------------------------------------------------------*/
    private readonly            Type                            _attributeFlag                  = null;

    /*==========================================================================================================================
    | CONSTRUCTOR (STATIC)
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Initializes static properties on <see cref="TypeCollection"/>.
    /// </summary>
    static TypeMemberInfoCollection() {
      SettableTypes = new List<Type> {
        typeof(bool),
        typeof(int),
        typeof(string),
        typeof(DateTime)
      };
    }


    /*==========================================================================================================================
    | CONSTRUCTOR
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Initializes a new instance of the <see cref="TypeCollection"/> class.
    /// </summary>
    /// <param name="attributeFlag">
    ///   An optional <see cref="System.Attribute"/> which properties must have defined to be considered writable.
    /// </param>
    internal TypeMemberInfoCollection(Type attributeFlag = null) : base() {
      _attributeFlag = attributeFlag;
    }

    /*==========================================================================================================================
    | METHOD: GET MEMBERS
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Returns a collection of <see cref="MemberInfo"/> objects associated with a specific type.
    /// </summary>
    /// <remarks>
    ///   If the collection cannot be found locally, it will be created.
    /// </remarks>
    /// <param name="type">The type for which the members should be retrieved.</param>
    internal MemberInfoCollection GetMembers(Type type) {
      if (!Contains(type)) {
        Add(new MemberInfoCollection(type));
      }
      return this[type];
    }

    /*==========================================================================================================================
    | METHOD: GET MEMBERS {T}
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Returns a collection of <typeparamref name="T"/> objects associated with a specific type.
    /// </summary>
    /// <remarks>
    ///   If the collection cannot be found locally, it will be created.
    /// </remarks>
    /// <param name="type">The type for which the members should be retrieved.</param>
    internal MemberInfoCollection<T> GetMembers<T>(Type type) where T: MemberInfo =>
      new MemberInfoCollection<T>(type, GetMembers(type).Where(m => typeof(T).IsAssignableFrom(m.GetType())).Cast<T>());

    /*==========================================================================================================================
    | METHOD: GET MEMBER
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Used reflection to identify a local member by a given name, and returns the associated <see cref="MemberInfo"/>
    ///   instance.
    /// </summary>
    internal MemberInfo GetMember(Type type, string name)  {
      var members = GetMembers(type);
      if (members.Contains(name)) {
        return members[name];
      }
      return null;
    }

    /*==========================================================================================================================
    | METHOD: GET MEMBER {T}
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Used reflection to identify a local member by a given name, and returns the associated <typeparamref name="T"/>
    ///   instance.
    /// </summary>
    internal T GetMember<T>(Type type, string name) where T : MemberInfo {
      var members = GetMembers(type);
      if (members.Contains(name) && typeof(T).IsAssignableFrom(members[name].GetType())) {
        return members[name] as T;
      }
      return null;
    }

    /*==========================================================================================================================
    | METHOD: HAS MEMBER
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Used reflection to identify if a local member is available.
    /// </summary>
    internal bool HasMember(Type type, string name) => GetMember(type, name) != null;

    /*==========================================================================================================================
    | METHOD: HAS MEMBER {T}
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Used reflection to identify if a local member of type <typeparamref name="T"/> is available.
    /// </summary>
    internal bool HasMember<T>(Type type, string name) where T: MemberInfo => GetMember<T>(type, name) != null;

    /*==========================================================================================================================
    | METHOD: HAS SETTABLE PROPERTY
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Used reflection to identify if a local property is available and settable.
    /// </summary>
    /// <remarks>
    ///   Will return false if the property is not available.
    /// </remarks>
    /// <param name="type">The <see cref="Type"/> on which the property is defined.</param>
    /// <param name="name">The name of the property to assess.</param>
    internal bool HasSettableProperty(Type type, string name) {
      var property = GetMember<PropertyInfo>(type, name);
      return (
        property != null &&
        property.CanWrite &&
        IsSettableType(property.PropertyType) &&
        (_attributeFlag == null || System.Attribute.IsDefined(property, _attributeFlag))
      );
    }

    /*==========================================================================================================================
    | METHOD: SET PROPERTY VALUE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Uses reflection to call a property, assuming that it is a) writable, and b) of type <see cref="String"/>,
    ///   <see cref="Int32"/>, or <see cref="Boolean"/>.
    /// </summary>
    /// <param name="target">The object on which the property is defined.</param>
    /// <param name="name">The name of the property to assess.</param>
    /// <param name="value">The value to set on the property.</param>
    internal bool SetPropertyValue(object target, string name, string value) {

      if (!HasSettableProperty(target.GetType(), name)) {
        return false;
      }

      var property = GetMember<PropertyInfo>(target.GetType(), name);

      Contract.Assume<InvalidOperationException>(property != null, $"The {name} property could not be retrieved.");

      var valueObject = GetValueObject(property.PropertyType, value);

      if (valueObject == null) {
        return false;
      }

      property.SetValue(target, valueObject);
      return true;

    }

    /*==========================================================================================================================
    | METHOD: HAS GETTABLE PROPERTY
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Used reflection to identify if a local property is available and gettable.
    /// </summary>
    /// <remarks>
    ///   Will return false if the property is not available.
    /// </remarks>
    /// <param name="type">The <see cref="Type"/> on which the property is defined.</param>
    /// <param name="name">The name of the property to assess.</param>
    /// <param name="targetType">Optional, the <see cref="Type"/> expected.</param>
    internal bool HasGettableProperty(Type type, string name, Type targetType = null) {
      var property = GetMember<PropertyInfo>(type, name);
      return (
        property != null &&
        property.CanRead &&
        IsSettableType(property.PropertyType, targetType) &&
        (_attributeFlag == null || System.Attribute.IsDefined(property, _attributeFlag))
      );
    }

    /*==========================================================================================================================
    | METHOD: GET PROPERTY VALUE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Uses reflection to call a property, assuming that it is a) readable, and b) of type <see cref="String"/>,
    ///   <see cref="Int32"/>, or <see cref="Boolean"/>.
    /// </summary>
    /// <param name="target">The object instance on which the property is defined.</param>
    /// <param name="name">The name of the property to assess.</param>
    /// <param name="targetType">Optional, the <see cref="Type"/> expected.</param>
    internal object GetPropertyValue(object target, string name, Type targetType = null) {

      if (!HasGettableProperty(target.GetType(), name, targetType)) {
        return null;
      }

      var property = GetMember<PropertyInfo>(target.GetType(), name);

      Contract.Assume<InvalidOperationException>(property != null, $"The {name} property could not be retrieved.");

      return property.GetValue(target);

    }

    /*==========================================================================================================================
    | METHOD: HAS SETTABLE METHOD
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Used reflection to identify if a local method is available and settable.
    /// </summary>
    /// <remarks>
    ///   Will return false if the method is not available. Methods are only considered settable if they have one parameter of
    ///   a settable type.
    /// </remarks>
    /// <param name="type">The <see cref="Type"/> on which the method is defined.</param>
    /// <param name="name">The name of the method to assess.</param>
    internal bool HasSettableMethod(Type type, string name) {
      var method = GetMember<MethodInfo>(type, name);
      return (
        method != null &&
        method.GetParameters().Count().Equals(1) &&
        IsSettableType(method.GetParameters().First().ParameterType) &&
        (_attributeFlag == null || System.Attribute.IsDefined(method, _attributeFlag))
      );
    }

    /*==========================================================================================================================
    | METHOD: SET METHOD VALUE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Uses reflection to call a method, assuming that it is a) writable, and b) of type <see cref="String"/>,
    ///   <see cref="Int32"/>, or <see cref="Boolean"/>.
    /// </summary>
    /// <param name="target">The object instance on which the method is defined.</param>
    /// <param name="name">The name of the method to assess.</param>
    /// <param name="value">The value to set the method to.</param>
    internal bool SetMethodValue(object target, string name, string value) {

      if (!HasSettableMethod(target.GetType(), name)) {
        return false;
      }

      var method = GetMember<MethodInfo>(target.GetType(), name);

      Contract.Assume<InvalidOperationException>(method != null, $"The {name}() method could not be retrieved.");

      var valueObject = GetValueObject(method.GetParameters().First().ParameterType, value);

      if (valueObject == null) {
        return false;
      }

      method.Invoke(target, new object[] {valueObject});

      return true;

    }

    /*==========================================================================================================================
    | METHOD: HAS GETTABLE METHOD
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Used reflection to identify if a local method is available and gettable.
    /// </summary>
    /// <remarks>
    ///   Will return false if the method is not available. Methods are only considered gettable if they have no parameters and
    ///   their return value is a settable type.
    /// </remarks>
    /// <param name="type">The <see cref="Type"/> on which the method is defined.</param>
    /// <param name="name">The name of the method to assess.</param>
    /// <param name="targetType">Optional, the <see cref="Type"/> expected.</param>
    internal bool HasGettableMethod(Type type, string name, Type targetType = null) {
      var method = GetMember<MethodInfo>(type, name);
      return (
        method != null &&
        method.GetParameters().Count().Equals(0) &&
        IsSettableType(method.ReturnType, targetType) &&
        (_attributeFlag == null || System.Attribute.IsDefined(method, _attributeFlag))
      );
    }

    /*==========================================================================================================================
    | METHOD: GET METHOD VALUE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Uses reflection to call a method, assuming that it has no parameters.
    /// </summary>
    /// <param name="target">The object instance on which the method is defined.</param>
    /// <param name="name">The name of the method to assess.</param>
    /// <param name="targetType">Optional, the <see cref="Type"/> expected.</param>
    internal object GetMethodValue(object target, string name, Type targetType = null) {

      if (!HasGettableMethod(target.GetType(), name, targetType)) {
        return null;
      }

      var method = GetMember<MethodInfo>(target.GetType(), name);

      return method.Invoke(target, Array.Empty<object>());

    }

    /*==========================================================================================================================
    | METHOD: IS SETTABLE TYPE?
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Determines whether a given type is settable, either assuming the list of <see cref="SettableTypes"/>, or provided a
    ///   specific <paramref name="targetType"/>.
    /// </summary>
    private static bool IsSettableType(Type sourceType, Type targetType = null) {

      if (targetType != null) {
        return sourceType.Equals(targetType);
      }
      return SettableTypes.Contains(sourceType);

    }

    /*==========================================================================================================================
    | METHOD: GET VALUE OBJECT
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Converts a string value to an object of the target type.
    /// </summary>
    private static object GetValueObject(Type type, string value) {

      var valueObject = (object)null;

      if (type.Equals(typeof(bool))) {
        valueObject = value == "1" || value.Equals("true", StringComparison.InvariantCultureIgnoreCase);
      }
      else if (type.Equals(typeof(int))) {
        int intValue = Int32.TryParse(value, out intValue)? intValue : 0;
        valueObject = intValue;
      }
      else if (type.Equals(typeof(string))) {
        valueObject = value;
      }
      else if (type.Equals(typeof(DateTime))) {
        if (DateTime.TryParse(value, out var date)) {
          valueObject = date;
        }
      }

      return valueObject;

    }

    /*==========================================================================================================================
    | PROPERTY: SETTABLE TYPES
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   A list of types that are allowed to be set using <see cref="SetPropertyValue(Object, String, String)"/>.
    /// </summary>
    static internal List<Type> SettableTypes { get; }

    /*==========================================================================================================================
    | OVERRIDE: INSERT ITEM
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Fires any time an item is added to the collection.
    /// </summary>
    /// <remarks>
    ///   Compared to the base implementation, will throw a specific <see cref="ArgumentException" /> error if a duplicate key
    ///   is inserted. This conveniently provides the <see cref="MemberInfoCollection{MemberInfo}.Type" />, so it's clear what
    ///   is being duplicated.
    /// </remarks>
    /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
    /// <param name="item">The <see cref="MemberInfoCollection" /> instance to insert.</param>
    /// <exception cref="ArgumentException">
    ///   The TypeMemberInfoCollection already contains the MemberInfoCollection of the Type '{item.Type}'.
    /// </exception>
    protected override void InsertItem(int index, MemberInfoCollection item) {
      if (!Contains(item.Type)) {
        base.InsertItem(index, item);
      }
      else {
        throw new ArgumentException(
          $"The '{nameof(TypeMemberInfoCollection)}' already contains the {nameof(MemberInfoCollection)} of the Type '{item.Type}'.");
      }
    }

    /*==========================================================================================================================
    | OVERRIDE: GET KEY FOR ITEM
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Method must be overridden for the EntityCollection to extract the keys from the items.
    /// </summary>
    /// <param name="item">The <see cref="Topic"/> object from which to extract the key.</param>
    /// <returns>The key for the specified collection item.</returns>
    protected override Type GetKeyForItem(MemberInfoCollection item) {
      Contract.Requires<ArgumentNullException>(item != null, "The item must be available in order to derive its key.");
      return item.Type;
    }

  } //Class

} //Namespace