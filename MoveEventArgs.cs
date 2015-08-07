namespace Ignia.Topics {

/*=========================================================================================================================
| MOVE EVENT ARGS
|
| Author:    Jeremy Caney, Ignia LLC (Jeremy.Caney@Ignia.com)
| Client     Ignia
| Project    Topics Library
|
| Purpose :  The MoveEventArgs object defines an event argument type specific to move events.  Allows tracking of the
|            source and destination topics.
|
>=========================================================================================================================
| Revisions  Date        Author          Comments
| - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
|            07.22.09    Jeremy Caney    Created initial version.
\------------------------------------------------------------------------------------------------------------------------*/

/*=========================================================================================================================
| NAMESPACES
\------------------------------------------------------------------------------------------------------------------------*/
  using System;
  using System.Collections.Generic;

/*=========================================================================================================================
| CLASS
\------------------------------------------------------------------------------------------------------------------------*/
  public class MoveEventArgs : EventArgs {

  /*------------------------------------------------------------------------------------------------------------------------
  | PRIVATE VARIABLES
  \------------------------------------------------------------------------------------------------------------------------*/
    private Topic   _topic = null;
    private Topic   _target = null;

  /*------------------------------------------------------------------------------------------------------------------------
  | CONSTRUCTOR: TAXONOMY MOVE EVENT ARGS
  >-------------------------------------------------------------------------------------------------------------------------
  | Constructor for a move event args object.
  \------------------------------------------------------------------------------------------------------------------------*/
    public MoveEventArgs() {
      }

    public MoveEventArgs(Topic topic, Topic target) {
      _topic = topic;
      _target = target;
      }

  /*------------------------------------------------------------------------------------------------------------------------
  | PROPERTY: EVENT TOPIC
  >-------------------------------------------------------------------------------------------------------------------------
  | Getter that returns the Topic object associated with the event
  \------------------------------------------------------------------------------------------------------------------------*/
    public Topic Topic {
      get {
        return _topic;
        }
      set {
        _topic = value;
        }
      }

  /*------------------------------------------------------------------------------------------------------------------------
  | PROPERTY: TARGET
  >-------------------------------------------------------------------------------------------------------------------------
  | Getter that returns the new parent that the topic will be moved to
  \------------------------------------------------------------------------------------------------------------------------*/
    public Topic Target {
      get {
        return _target;
        }
      set {
        _target = value;
        }
      }

    } //Class
  } //Namespace