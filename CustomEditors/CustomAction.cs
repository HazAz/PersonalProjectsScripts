using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Custom actions.
/// Have multiple actions, each with its own parameters.
/// Each action has a delegate to the next action for gizmos.
/// </summary>
public class CustomAction : MonoBehaviour
{
   public enum ActionType
    {
        Move,
        Talk,
        OpenDoor,
    }

    public ActionType actionType;

    [System.Serializable]
    public class MoveParams
    {
        public GameObject character;
        public Transform target;
        public float speed;
        public CustomAction nextAction;
        public void ExecuteAction()
        {
            if (nextAction != null) character.GetComponent<Movement>().MoveTo(target, speed, nextAction.ExecuteAction);
            else character.GetComponent<Movement>().MoveTo(target,speed);
        }
    }

    public MoveParams moveParams;

    [System.Serializable]
    public class TalkParams
    {
        public GameObject gameObject;
        public string text;
        public CustomAction nextAction;
        public void ExecuteAction()
        { }
    }

    public TalkParams talkParams;

    [System.Serializable]
    public class OpenDoorParams
    {
        public GameObject door;
        public float waitTime;
        public CustomAction nextAction;

        public void ExecuteAction()
        {
            if (nextAction != null) door.GetComponent<OpenDoor>().OpenDoors(waitTime, nextAction.ExecuteAction);
            else door.GetComponent<OpenDoor>().OpenDoors(waitTime);
        }
    }

    public OpenDoorParams openDoorParams;

    public void ExecuteAction()
    {
        switch (actionType)
        {
            case ActionType.Move:
                moveParams.ExecuteAction();
                break;

            case ActionType.Talk:
                talkParams.ExecuteAction();
                break;

            case ActionType.OpenDoor:
                openDoorParams.ExecuteAction();
                break;
        }
    }

#if UNITY_EDITOR
    // GIZMOS
    /// <summary>
    /// Draws lines where we need them to help understand whats going on.
    /// </summary>
    private void OnDrawGizmos()
    {
        // Label
        Handles.Label(transform.position, "" + actionType); // Uses UnityEditor

        switch (actionType)
        {
            case ActionType.Talk:
                if (talkParams.gameObject != null)
                {
                    GizmoDrawLine(transform.position, talkParams.gameObject.transform.position, Color.yellow, "talk", Color.white, false);
                }
                if (talkParams.nextAction != null)
                {
                    GizmoDrawLine(transform.position, talkParams.nextAction.transform.position, Color.black, "next:", Color.black, true);
                }
                break;

            case ActionType.Move:
                if (moveParams.character != null)
                {
                    GizmoDrawLine(transform.position, moveParams.character.transform.position, Color.yellow, "Move character", Color.white, false);
                }
                Handles.color = Color.red;
                if (moveParams.character != null && moveParams.target != null)
                {
                    GizmoDrawLine(transform.position, moveParams.target.transform.position, Color.green, "Move here", Color.white, false);
                }
                if (moveParams.nextAction != null)
                {
                    GizmoDrawLine(transform.position, moveParams.nextAction.transform.position, Color.black, "next:", Color.black, true);
                }
                break;

            case ActionType.OpenDoor:
                if (openDoorParams.door != null)
                {
                    GizmoDrawLine(transform.position, openDoorParams.door.transform.position, Color.red, "Open door", Color.white, false);
                }
                if (openDoorParams.nextAction != null)
                {
                    GizmoDrawLine(transform.position, openDoorParams.nextAction.transform.position, Color.black, "next:", Color.black, true);
                }
                break;
        }
    }
    /// <summary>
    /// Helper method to draw the line. Takes a start point, end point, a line color, a text to write at the middle, text color
    /// and a bool that determines if the text is at the center or at the end
    /// </summary>
    private void GizmoDrawLine(Vector3 start, Vector3 finish, Color lineColor, string text, Color textColor, bool textCenter)
    {
        Vector3 dir = (finish - start).normalized;
        float distance = Vector3.Distance(start, finish);

        Handles.color = lineColor;
        Handles.DrawAAPolyLine(5f, start, finish);

        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        style.normal.textColor = textColor;
        if (textCenter)
        {
            Handles.Label((start + finish) * 0.5f, text, style);
        }
        else
        {
            Handles.Label(finish, text, style);
        }
    }
#endif

    
    // CONTEXT MENU
    /// <summary>
    /// This adds Context menu 
    /// </summary>
    [ContextMenu("Reset To Default Variables")]
    private void ResetToDefaultVariables()
    {
        Debug.Log("Resetting to default");
        moveParams = new MoveParams();
        openDoorParams = new OpenDoorParams();
        talkParams = new TalkParams();
    }
}
