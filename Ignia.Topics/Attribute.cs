﻿/*==============================================================================================================================
| Author        Ignia, LLC
| Client        Ignia, LLC
| Project       Topics Library
\=============================================================================================================================*/
using System;
using System.Collections.Generic;

namespace Ignia.Topics {

  /*============================================================================================================================
  | CLASS: ATTRIBUTE
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   Represents metadata for describing an attribute, including information on how it will be presented and validated in the
  ///   editor.
  /// </summary>
  /// <remarks>
  ///   The <see cref="Attribute"/> class is provided exclusively for backward compatibility with attributes of the content type
  ///   "Attribute" so that the <see cref="TopicFactory.Create(String, String, Topic)"/> factory method can find a match. Databases
  ///   should be updated to instead use "AttributeDescriptor", which better differentiates the metadata from the
  ///   <see cref="AttributeValue"/>. The <see cref="Attribute"/> class derives from <see cref="AttributeDescriptor"/> to ensure
  ///   they support the same functionality.
  /// </remarks>
  [Obsolete("The Attribute class is deprecated. Topic databases and code should be updated to use AttributeDescriptor instead")]
  public class Attribute : AttributeDescriptor {

    /*==========================================================================================================================
    | CONSTRUCTOR
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Initializes a new instance of the <see cref="Attribute"/> class.
    /// </summary>
    public Attribute() : base() { }

  } //Class

} //Namespace