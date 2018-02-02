using Framework.Events;
using Framework.RuntimeSet;
using UnityEngine;

namespace Absolute.VR.Inputs
{
    /// <summary>
    /// Set the GameEvent depending on the Vive Inputs
    /// </summary>
    public class ViveInputCapture : MonoBehaviour
    {
        #region PUBLIC_VARIABLES
        [Header("SteamVR Tracked Object from the two Controllers")]
        [HideInInspector]
        public SteamVR_TrackedObject LeftTrackedObject;
        [HideInInspector]
        public SteamVR_TrackedObject RightTrackedObject;

        [Header("Left Controller GameEvents Dictionnary")]
        public VRInputsEvents LeftEventsDictionnary;

        [Header("Right Controller GameEvents Dictionnary")]
        public VRInputsEvents RightEventsDictionnary;
        #endregion PUBLIC_VARIABLES

        #region PRIVATE_VARIABLES
        private SteamVR_Controller.Device LeftController
        {
            get { return SteamVR_Controller.Input((int)LeftTrackedObject.index); }
        }

        private SteamVR_Controller.Device RightController
        {
            get { return SteamVR_Controller.Input((int)RightTrackedObject.index); }
        }

        #region Left_Controller_Variables
        private bool leftTriggerIsDown;
        private bool leftMenuIsDown;
        private bool leftGripIsDown;
        private bool leftThumbIsDown;

        GameEvent leftBasicEvent;
        GameEventBool leftBoolEvent;
        GameEventVector3 leftVector3Event;
        #endregion Left_Controller_Variables

        #region Right_Controller_Variables
        private bool rightTriggerIsDown;
        private bool rightMenuIsDown;
        private bool rightGripIsDown;
        private bool rightThumbIsDown;

        GameEvent rightBasicEvent;
        GameEventBool rightBoolEvent;
        GameEventVector3 rightVector3Event;
        #endregion Right_Controller_Variables

        #endregion PRIVATE_VARIABLES

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
        void Update()
        {
            CheckLeftControllerInput();
            CheckRightControllerInput();
        }
        #endregion MONOBEHAVIOURS

        //EMPTY
        #region PUBLIC_METHODS
        #endregion PUBLIC_METHODS

        #region PRIVATE_METHODS
        /// <summary>
        /// Handle the Left Controller input and put them in the Events
        /// </summary>
        void CheckLeftControllerInput()
        {
            // ------------------------------------------------------ Trigger ------------------------------------------------------ 
            if (LeftController.GetHairTriggerDown())
            {
                leftTriggerIsDown = true;
                leftBasicEvent = (GameEvent)LeftEventsDictionnary.Get("LeftTriggerDown");
                leftBasicEvent.Raise();
            }
            else if (LeftController.GetHairTriggerUp())
            {
                leftTriggerIsDown = false;
                leftBasicEvent = (GameEvent)LeftEventsDictionnary.Get("LeftTriggerUp");
                leftBasicEvent.Raise();
            }

            // ------------------------------------------------------ Touchpad ------------------------------------------------------ 
            if (LeftController.GetAxis() != Vector2.zero)
            {
                leftVector3Event = (GameEventVector3)LeftEventsDictionnary.Get("LeftThumbOrientation");
                leftVector3Event.Raise(LeftController.GetAxis());
            }

            if (LeftController.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
            {
                leftThumbIsDown = true;
                leftBasicEvent = (GameEvent)LeftEventsDictionnary.Get("LeftThumbDown");
                leftBasicEvent.Raise();
            }
            else if (LeftController.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
            {
                leftThumbIsDown = false;
                leftBasicEvent = (GameEvent)LeftEventsDictionnary.Get("LeftThumbUp");
                leftBasicEvent.Raise();
            }

            if (LeftController.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad))
            {
                leftBoolEvent = (GameEventBool)LeftEventsDictionnary.Get("LeftThumbTouching");
                leftBoolEvent.Raise(true);
            }
            else if (LeftController.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
            {
                leftBoolEvent = (GameEventBool)LeftEventsDictionnary.Get("LeftThumbTouching");
                leftBoolEvent.Raise(false);
            }

            // ------------------------------------------------------ Grip ------------------------------------------------------ 
            if (LeftController.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
            {
                leftGripIsDown = true;
                leftBasicEvent = (GameEvent)LeftEventsDictionnary.Get("LeftGripDown");
                leftBasicEvent.Raise();
            }
            else if (LeftController.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
            {
                leftGripIsDown = false;
                leftBasicEvent = (GameEvent)LeftEventsDictionnary.Get("LeftGripUp");
                leftBasicEvent.Raise();
            }


            // ------------------------------------------------------ Menu ------------------------------------------------------ 
            if (LeftController.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
            {
                leftBasicEvent = (GameEvent)LeftEventsDictionnary.Get("LeftMenuDown");
                leftBasicEvent.Raise();
            }
            else if (LeftController.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu))
            {
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
            if (RightController.GetHairTriggerDown())
            {
                rightTriggerIsDown = true;
                rightBasicEvent = (GameEvent)RightEventsDictionnary.Get("RightTriggerDown");
                rightBasicEvent.Raise();
            }
            else if (RightController.GetHairTriggerUp())
            {
                rightTriggerIsDown = false;
                rightBasicEvent = (GameEvent)RightEventsDictionnary.Get("RightTriggerUp");
                rightBasicEvent.Raise();
            }

            // ------------------------------------------------------ Touchpad ------------------------------------------------------ 
            if (RightController.GetAxis() != Vector2.zero)
            {
                rightVector3Event = (GameEventVector3)RightEventsDictionnary.Get("RightThumbOrientation");
                rightVector3Event.Raise(RightController.GetAxis());
            }

            if (!rightThumbIsDown && RightController.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
            {
                rightThumbIsDown = true;
                rightBasicEvent = (GameEvent)RightEventsDictionnary.Get("RightThumbDown");
                rightBasicEvent.Raise();
            }
            else if (rightThumbIsDown && !RightController.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
            {
                rightThumbIsDown = false;
                rightBasicEvent = (GameEvent)RightEventsDictionnary.Get("RightThumbUp");
                rightBasicEvent.Raise();
            }

            if (RightController.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad))
            {
                rightBoolEvent = (GameEventBool)RightEventsDictionnary.Get("RightThumbTouching");
                rightBoolEvent.Raise(true);
            }
            else if (RightController.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
            {
                rightBoolEvent = (GameEventBool)RightEventsDictionnary.Get("RightThumbTouching");
                rightBoolEvent.Raise(false);
            }

            // ------------------------------------------------------ Grip ------------------------------------------------------ 
            if (RightController.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
            {
                rightGripIsDown = true;
                rightBasicEvent = (GameEvent)RightEventsDictionnary.Get("RightGripDown");
                rightBasicEvent.Raise();
            }
            else if (RightController.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
            {
                rightGripIsDown = false;
                rightBasicEvent = (GameEvent)RightEventsDictionnary.Get("RightGripUp");
                rightBasicEvent.Raise();
            }

            // ------------------------------------------------------ Menu ------------------------------------------------------ 
            if (RightController.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
            {
                rightMenuIsDown = true;
                rightBasicEvent = (GameEvent)RightEventsDictionnary.Get("RightMenuDown");
                rightBasicEvent.Raise();
            }
            else if (RightController.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu))
            {
                rightMenuIsDown = false;
                rightBasicEvent = (GameEvent)RightEventsDictionnary.Get("RightMenuUp");
                rightBasicEvent.Raise();
            }
        }
        #endregion PRIVATE_METHODS
    }
}
