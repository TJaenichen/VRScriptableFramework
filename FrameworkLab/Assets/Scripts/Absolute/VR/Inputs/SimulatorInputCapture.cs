using Framework.Events;
using Framework.RuntimeSet;
using UnityEngine;

/// <summary>
/// Set the GameEvent depending on the Keyboard and Mouse Inputs
/// </summary>
public class SimulatorInputCapture : MonoBehaviour
{
    #region PUBLIC_VARIABLES
    [Header("Left Controller GameEvents Dictionnary")]
    public VRInputs LeftEventsDictionnary;

    [Header("Right Controller GameEvents Dictionnary")]
    public VRInputs RightEventsDictionnary;
    #endregion

    #region PRIVATE_VARIABLES

    #region Left_Controller_Variables
    private bool leftTriggerIsDown;
    private bool leftMenuIsDown;
    private bool leftGripIsDown;
    private bool leftThumbIsDown;
    private bool leftThumbIsTouching;
    GameEvent leftBasicEvent;
    GameEventBool leftBoolEvent;
    GameEventVector3 leftVector3Event;
    #endregion Left_Controller_Variables

    #region Right_Controller_Variables
    private bool rightTriggerIsDown;
    private bool rightMenuIsDown;
    private bool rightGripIsDown;
    private bool rightThumbIsTouching;
    private bool rightThumbIsDown;
    GameEvent rightBasicEvent;
    GameEventBool rightBoolEvent;
    GameEventVector3 rightVector3Event;
    #endregion Right_Controller_Variables

    #endregion

    #region MONOBEHAVIOURS
    // Update is called once per frame
    void Update()
    {
        CheckLeftControllerInput();
        CheckRightControllerInput();
    }
    #endregion

    //EMPTY
    #region PUBLIC_METHODS
    #endregion

    #region PRIVATE_METHODS
    /// <summary>
    /// Handle the Left Controller input and put them in the Events
    /// </summary>
    void CheckLeftControllerInput()
    {
        // ------------------------------------------------------ Trigger ------------------------------------------------------ 
        //if (!leftTriggerIsDown && OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        //{
        //    leftTriggerIsDown = true;
        //    leftBasicEvent = (GameEvent)LeftEventsDictionnary.Get("LeftTriggerDown");
        //    leftBasicEvent.Raise();
        //}
        //else if (leftTriggerIsDown && !OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        //{
        //    leftTriggerIsDown = false;
        //    leftBasicEvent = (GameEvent)LeftEventsDictionnary.Get("LeftTriggerUp");
        //    leftBasicEvent.Raise();
        //}

        //for now, only one trigger is set 

        // ------------------------------------------------------ Thumbstick ------------------------------------------------------ 
        //close/open options
        if (Input.GetKeyDown(KeyCode.S))
        {
            leftThumbIsDown = true;
            leftVector3Event = (GameEventVector3)LeftEventsDictionnary.Get("LeftThumbOrientation");
            leftVector3Event.Raise(Vector3.down);

            leftBasicEvent = (GameEvent)LeftEventsDictionnary.Get("LeftThumbDown");
            leftBasicEvent.Raise();
        }
        //close/open project list
        else if (Input.GetKeyDown(KeyCode.D))
        {
            leftThumbIsDown = true;
            leftVector3Event = (GameEventVector3)LeftEventsDictionnary.Get("LeftThumbOrientation");
            leftVector3Event.Raise(Vector3.right);

            leftBasicEvent = (GameEvent)LeftEventsDictionnary.Get("LeftThumbDown");
            leftBasicEvent.Raise();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            leftThumbIsDown = true;
            leftVector3Event = (GameEventVector3)LeftEventsDictionnary.Get("LeftThumbOrientation");
            leftVector3Event.Raise(Vector3.left);

            leftBasicEvent = (GameEvent)LeftEventsDictionnary.Get("LeftThumbDown");
            leftBasicEvent.Raise();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            leftThumbIsDown = true;
            leftVector3Event = (GameEventVector3)LeftEventsDictionnary.Get("LeftThumbOrientation");
            leftVector3Event.Raise(Vector3.up);

            leftBasicEvent = (GameEvent)LeftEventsDictionnary.Get("LeftThumbDown");
            leftBasicEvent.Raise();
        }

        // ------------------------------------------------------ Grip ------------------------------------------------------ 
        if (!leftGripIsDown && Input.GetKeyDown(KeyCode.Tab))
        {
            leftGripIsDown = true;
            leftBasicEvent = (GameEvent)LeftEventsDictionnary.Get("LeftGripDown");
            leftBasicEvent.Raise();
        }
        else if (leftGripIsDown && !Input.GetKeyDown(KeyCode.Tab))
        {
            leftGripIsDown = false;
            leftBasicEvent = (GameEvent)LeftEventsDictionnary.Get("LeftGripUp");
            leftBasicEvent.Raise();
        }

        // ------------------------------------------------------ Menu ------------------------------------------------------ 
        //if (!leftMenuIsDown && OVRInput.Get(OVRInput.Button.Start))
        //{
        //    leftMenuIsDown = true;
        //    leftBasicEvent = (GameEvent)LeftEventsDictionnary.Get("LeftMenuDown");
        //    leftBasicEvent.Raise();
        //}
        //else if (leftMenuIsDown && !OVRInput.Get(OVRInput.Button.Start))
        //{
        //    leftMenuIsDown = false;
        //    leftBasicEvent = (GameEvent)LeftEventsDictionnary.Get("LeftMenuUp");
        //    leftBasicEvent.Raise();
        //}
    }

    /// <summary>
    /// Handle the Right Controller input and put them in the Events
    /// </summary>
    void CheckRightControllerInput()
    {
        // ------------------------------------------------------ Trigger ------------------------------------------------------ 
        if (!rightTriggerIsDown && Input.GetMouseButtonDown(0))
        {
            rightTriggerIsDown = true;
            rightBasicEvent = (GameEvent)RightEventsDictionnary.Get("RightTriggerDown");
            rightBasicEvent.Raise();
        }
        else if (rightTriggerIsDown && Input.GetMouseButtonUp(0))
        {
            rightTriggerIsDown = false;
            rightBasicEvent = (GameEvent)RightEventsDictionnary.Get("RightTriggerUp");
            rightBasicEvent.Raise();
        }

        // ------------------------------------------------------ Thumbstick ------------------------------------------------------ 
        if (!rightThumbIsDown && Input.GetKeyDown(KeyCode.UpArrow))
        {
            rightVector3Event = (GameEventVector3)RightEventsDictionnary.Get("RightThumbOrientation");
            rightVector3Event.Raise(Vector3.forward);

            rightThumbIsDown = true;
            rightBasicEvent = (GameEvent)RightEventsDictionnary.Get("RightThumbDown");
            rightBasicEvent.Raise();
        }
        else if (rightThumbIsDown && Input.GetKeyUp(KeyCode.UpArrow))
        {
            rightVector3Event = (GameEventVector3)RightEventsDictionnary.Get("RightThumbOrientation");
            rightVector3Event.Raise(Vector3.zero);

            rightThumbIsDown = false;
            rightBasicEvent = (GameEvent)RightEventsDictionnary.Get("RightThumbUp");
            rightBasicEvent.Raise();
        }

        // ------------------------------------------------------ Grip ------------------------------------------------------ 
        if (!rightGripIsDown && Input.GetMouseButton(1))
        {
            rightGripIsDown = true;
            rightBasicEvent = (GameEvent)RightEventsDictionnary.Get("RightGripDown");
            rightBasicEvent.Raise();
        }
        else if (rightGripIsDown && !Input.GetMouseButton(1))
        {
            rightGripIsDown = false;
            rightBasicEvent = (GameEvent)RightEventsDictionnary.Get("RightGripUp");
            rightBasicEvent.Raise();
        }
    }
    #endregion
}
