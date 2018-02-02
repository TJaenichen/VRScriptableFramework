using Framework.Events;
using Framework.RuntimeSet;
using UnityEngine;

/// <summary>
/// Set the GameEvent depending on the Oculus Inputs
/// </summary>
public class OculusInputCapture : MonoBehaviour
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
    private void Start()
    {
        leftBasicEvent = new GameEvent();
        leftBoolEvent = new GameEventBool();
        leftVector3Event = new GameEventVector3();

        rightBasicEvent = new GameEvent();
        rightBoolEvent = new GameEventBool();
        rightVector3Event = new GameEventVector3();
    }

    // Update is called once per frame
    void Update ()
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
        if (!leftTriggerIsDown && OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            leftTriggerIsDown = true;
            leftBasicEvent = (GameEvent)LeftEventsDictionnary.Get("LeftTriggerDown");
            leftBasicEvent.Raise();
        }
        else if (leftTriggerIsDown && !OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            leftTriggerIsDown = false;
            leftBasicEvent = (GameEvent)LeftEventsDictionnary.Get("LeftTriggerUp");
            leftBasicEvent.Raise();
        }

        // ------------------------------------------------------ Thumbstick ------------------------------------------------------ 
        if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick) != Vector2.zero)
        {
            leftVector3Event = (GameEventVector3)LeftEventsDictionnary.Get("LeftThumbOrientation");
            leftVector3Event.Raise(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick));
        }

        if (!leftThumbIsDown && OVRInput.Get(OVRInput.Button.PrimaryThumbstick))
        {
            leftThumbIsDown = true;
            leftBasicEvent = (GameEvent)LeftEventsDictionnary.Get("LeftThumbDown");
            leftBasicEvent.Raise();
        }
        else if (leftThumbIsDown && !OVRInput.Get(OVRInput.Button.PrimaryThumbstick))
        {
            leftThumbIsDown = false;
            leftBasicEvent = (GameEvent)LeftEventsDictionnary.Get("LeftThumbUp");
            leftBasicEvent.Raise();
        }

        if (!leftThumbIsTouching && OVRInput.Get(OVRInput.Touch.PrimaryThumbstick))
        {
            leftThumbIsTouching = true;
            leftBoolEvent = (GameEventBool)LeftEventsDictionnary.Get("LeftThumbTouching");
            leftBoolEvent.Raise(true);
        }
        else if (leftThumbIsTouching && !OVRInput.Get(OVRInput.Touch.PrimaryThumbstick))
        {
            leftThumbIsTouching = false;
            leftBoolEvent = (GameEventBool)LeftEventsDictionnary.Get("LeftThumbTouching");
            leftBoolEvent.Raise(false);
        }

        // ------------------------------------------------------ Grip ------------------------------------------------------ 
        if (!leftGripIsDown && OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
        {
            leftGripIsDown = true;
            leftBasicEvent = (GameEvent)LeftEventsDictionnary.Get("LeftGripDown");
            leftBasicEvent.Raise();
        }
        else if (leftGripIsDown && !OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
        {
            leftGripIsDown = false;
            leftBasicEvent = (GameEvent)LeftEventsDictionnary.Get("LeftGripUp");
            leftBasicEvent.Raise();
        }

        // ------------------------------------------------------ Menu ------------------------------------------------------ 
        if (!leftMenuIsDown && OVRInput.Get(OVRInput.Button.Start))
        {
            leftMenuIsDown = true;
            leftBasicEvent = (GameEvent)LeftEventsDictionnary.Get("LeftMenuDown");
            leftBasicEvent.Raise();
        }
        else if (leftMenuIsDown && !OVRInput.Get(OVRInput.Button.Start))
        {
            leftMenuIsDown = false;
            leftBasicEvent = (GameEvent)LeftEventsDictionnary.Get("LeftMenuUp");
            leftBasicEvent.Raise();
        }
    }

    /// <summary>
    /// Handle the Right Controller input and put them in the Events
    /// </summary>
    void CheckRightControllerInput()
    {
        // ------------------------------------------------------ Trigger ------------------------------------------------------ 
        if (!rightTriggerIsDown && OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            rightTriggerIsDown = true;
            rightBasicEvent = (GameEvent)RightEventsDictionnary.Get("RightTriggerDown");
            rightBasicEvent.Raise();
        }
        else if (rightTriggerIsDown && !OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            rightTriggerIsDown = false;
            rightBasicEvent = (GameEvent)RightEventsDictionnary.Get("RightTriggerUp");
            rightBasicEvent.Raise();
        }

        // ------------------------------------------------------ Thumbstick ------------------------------------------------------ 
        if (OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick) != Vector2.zero)
        {
            rightVector3Event = (GameEventVector3)RightEventsDictionnary.Get("RightThumbOrientation");
            rightVector3Event.Raise(OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick));
        }

        if (!rightThumbIsDown && OVRInput.Get(OVRInput.Button.SecondaryThumbstick))
        {
            rightThumbIsDown = true;
            rightBasicEvent = (GameEvent)RightEventsDictionnary.Get("RightThumbDown");
            rightBasicEvent.Raise();
        }
        else if (rightThumbIsDown && !OVRInput.Get(OVRInput.Button.SecondaryThumbstick))
        {
            rightThumbIsDown = false;
            rightBasicEvent = (GameEvent)RightEventsDictionnary.Get("RightThumbUp");
            rightBasicEvent.Raise();
        }

        if (!rightThumbIsTouching && OVRInput.Get(OVRInput.Touch.SecondaryThumbstick))
        {
            rightThumbIsTouching = true;
            rightBoolEvent = (GameEventBool)RightEventsDictionnary.Get("RightThumbTouching");
            rightBoolEvent.Raise(true);
        }
        else if (rightThumbIsTouching && !OVRInput.Get(OVRInput.Touch.SecondaryThumbstick))
        {
            rightThumbIsTouching = false;
            rightBoolEvent = (GameEventBool)RightEventsDictionnary.Get("RightThumbTouching");
            rightBoolEvent.Raise(false);
        }

        // ------------------------------------------------------ Grip ------------------------------------------------------ 
        if (!rightGripIsDown && OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
        {
            rightGripIsDown = true;
            rightBasicEvent = (GameEvent)RightEventsDictionnary.Get("RightGripDown");
            rightBasicEvent.Raise();
        }
        else if (rightGripIsDown && !OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
        {
            rightGripIsDown = false;
            rightBasicEvent = (GameEvent)RightEventsDictionnary.Get("RightGripUp");
            rightBasicEvent.Raise();
        }

        //No Right menu button on the oculus

        ////Button B
        //if (!bButtonIsDown && OVRInput.Get(OVRInput.Button.Two))
        //{
        //    RightBButton.Raise();
        //    bButtonIsDown = true;
        //} else if (bButtonIsDown && !OVRInput.Get(OVRInput.Button.Two))
        //{
        //    //Can add here an event OnBButtonUp if necessary
        //    bButtonIsDown = false;
        //}
    }
    #endregion
}
