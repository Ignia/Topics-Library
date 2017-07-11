/*==============================================================================================================================
| Author        Ignia, LLC
| Client        Ignia, LLC
| Project       Topics Library
\=============================================================================================================================*/
using System;
using System.Diagnostics.Contracts;

namespace Ignia.Topics.Repositories {

  /*============================================================================================================================
  | CLASS: TOPIC DATA PROVIDER BASE CONTRACT
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   Defines code contracts for the <c>TopicDataProviderBase</c> class' abstract methods <c>Load()</c> and <c>Move()</c>.
  /// </summary>
  [ContractClassFor(typeof(TopicRepositoryBase))]
  public abstract class TopicRepositoryBaseContract : TopicRepositoryBase {

    /// <summary>
    ///   Interface method that loads topics into memory.
    /// </summary>
    /// <remarks>
    ///   Contract preconditions are defined in the <see cref="TopicRepositoryBaseContract"/> contract class.
    /// </remarks>
    protected override Topic Load(string topicKey, int topicId, int depth, DateTime? version = null) {
  
      /*------------------------------------------------------------------------------------------------------------------------
      | Provide dummy return value
      \-----------------------------------------------------------------------------------------------------------------------*/
      return new Topic();

    }

    /// <summary>
    ///   Interface method that moves the specified topic within the tree, usng the specified sibling topic object as a
    ///   secondary target for the move.
    /// </summary>
    /// <requires description="The topic to be moved must not be null." exception="T:System.ArgumentNullException">
    ///   topic != null
    /// </requires>
    /// <requires description="The target parent topic must not be null." exception="T:System.ArgumentNullException">
    ///   target != null
    /// </requires>
    /// <requires description="The topic to be moved cannot be its own parent." exception="T:System.ArgumentException">
    ///   topic != target
    /// </requires>
    /// <requires description="The topic cannot be moved relative to itself." exception="T:System.ArgumentException">
    ///   topic != sibling
    /// </requires>
    public override void Move(Topic topic, Topic target, Topic sibling) {

      /*------------------------------------------------------------------------------------------------------------------------
      | Validate parameters
      \-----------------------------------------------------------------------------------------------------------------------*/
      Contract.Requires<ArgumentNullException>(topic != null, "topic");
      Contract.Requires<ArgumentNullException>(target != null, "target");
      Contract.Requires<ArgumentException>(topic != target, "A topic cannot be its own parent.");
      Contract.Requires<ArgumentException>(topic != sibling, "A topic cannot be moved relative to itself.");

      /*------------------------------------------------------------------------------------------------------------------------
      | Provide dummy return value
      \-----------------------------------------------------------------------------------------------------------------------*/
      //return true;

    }

  } // Class

} // Namespace