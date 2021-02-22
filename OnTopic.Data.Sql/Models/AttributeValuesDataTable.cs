﻿/*==============================================================================================================================
| Author        Ignia, LLC
| Client        Ignia, LLC
| Project       Topics Library
\=============================================================================================================================*/
using System.Data;
using OnTopic.Attributes;
using OnTopic.Collections.Specialized;

namespace OnTopic.Data.Sql.Models {

  /*============================================================================================================================
  | CLASS: ATTRIBUTE VALUES (DATA TABLE)
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   Extends <see cref="DataTable"/> to model the schema for the <c>AttributeValues</c> user-defined table type.
  /// </summary>
  internal class AttributeValuesDataTable: DataTable {

    /*==========================================================================================================================
    | CONSTRUCTOR
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a new <see cref="DataTable"/> with the appropriate schema for the <c>AttributeValues</c> user-defined
    ///   table type.
    /// </summary>
    internal AttributeValuesDataTable() {

      /*------------------------------------------------------------------------------------------------------------------------
      | COLUMN: Attribute Key
      \-----------------------------------------------------------------------------------------------------------------------*/
      Columns.Add(
        new DataColumn("AttributeKey") {
          MaxLength             = 128
        }
      );

      /*------------------------------------------------------------------------------------------------------------------------
      | COLUMN: Attribute Value
      \-----------------------------------------------------------------------------------------------------------------------*/
      Columns.Add(
        new DataColumn("AttributeRecord") {
          MaxLength             = 255
        }
      );

    }

    /*==========================================================================================================================
    | ADD ROW
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Provides a convenience method for adding a new <see cref="DataRow"/> based on the expected column values.
    /// </summary>
    /// <param name="attributeKey">The <see cref="TrackedRecord{T}.Key"/>.</param>
    /// <param name="attributeValue">The <see cref="TrackedRecord{T}.Value"/>.</param>
    internal DataRow AddRow(string attributeKey, string? attributeValue = null) {

      /*------------------------------------------------------------------------------------------------------------------------
      | Define record
      \-----------------------------------------------------------------------------------------------------------------------*/
      var record                = NewRow();
      record["AttributeKey"]    = attributeKey;
      record["AttributeRecord"]  = attributeValue;

      /*------------------------------------------------------------------------------------------------------------------------
      | Add record
      \-----------------------------------------------------------------------------------------------------------------------*/
      Rows.Add(record);

      /*------------------------------------------------------------------------------------------------------------------------
      | Return record
      \-----------------------------------------------------------------------------------------------------------------------*/
      return record;

    }

  } //Class
} //Namespaces