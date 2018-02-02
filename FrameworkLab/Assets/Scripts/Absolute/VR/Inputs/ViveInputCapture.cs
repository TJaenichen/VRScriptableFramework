using Framework.Events;
using Framework.RuntimeSet;
using Framework.Variables;
using UnityEngine;

namespace Framework.VR.Inputs
{
    /// <summary>
    /// Set the GameEvent depending on the Vive Inputs
    /// </summary>
    public class ViveInputCapture : MonoBehaviour
    {
        #region PUBLIC_VARIABLES
        [Header("SteamVR Tracked Object from the two Controllers")]
        public SteamVR_TrackedObject LeftTrackedObject;
        public SteamVR_TrackedObject RightTrackedObject;

        [Header("Left Controller GameEvents Dictionnary")]
        public VRInputsEvents LeftEventsDictionnary;

        [Header("Right Controller GameEvents Dictionnary")]
        public VRInputsEvents RightEventsDictionnary;

        [Header("Left Controller BoolVariable Dictionnary")]
        public VRInputsBoolean LeftVariablesDictionnary;

        [Header("Right Controller BoolVariable Dictionnary")]
        public VRInputsBoolean RightVariablesDictionnary;

        [Header("Thumbs positions on the stick/touchpad")]
        public Vector3Variable LeftThumbOrientation;
        public Vector3Variable RightThumbOrientation;
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
        GameEvent _leftEvent;
        GameEventBool _leftEventBool;
        #endregion Left_Controller_Variables

        #region Right_Controller_Variables
        GameEvent _rightEvent;
        GameEventBool _rightEventBool;
        #endregion Right_Controller_Variables

        #endregion PRIVATE_VARIABLES

        #region MONOBEHAVIOURS
        private void Start()
        {
            _leftEvent = new GameEvent();
            _leftEventBool = new GameEventBool();

            _rightEvent = new GameEvent();
            _rightEventBool = new GameEventBool();
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
            BoolVariable temp;

            #region TRIGGER
            temp = LeftVariablesDictionnary.Get("TriggerIsDown");

            if (!temp.Value && LeftController.GetHairTriggerDown())
            {
                temp.SetValue(true);
                _leftEvent = (GameEvent)LeftEventsDictionnary.Get("LeftTriggerDown");
                _leftEvent.Raise();
            }
            else if (temp.Value && LeftController.GetHairTriggerUp())
            {
                temp.SetValue(false);
                _leftEvent = (GameEvent)LeftEventsDictionnary.Get("LeftTriggerUp");
                _leftEvent.Raise();
            }
            #endregion TRIGGER

            #region TOUCHPAD
            temp = LeftVariablesDictionnary.Get("ThumbIsDown");

            LeftThumbOrientation.SetValue(LeftController.GetAxis());
            
            if (LeftController.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
            {
                temp.SetValue(true);
                _leftEvent = (GameEvent)LeftEventsDictionnary.Get("LeftThumbDown");
                _leftEvent.Raise();
            }
            else if (temp.Value && LeftController.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
            {
                temp.SetValue(false);
                _leftEvent = (GameEvent)RightEventsDictionnary.Get("LeftThumbUp");
                _leftEvent.Raise();
            }

            if (LeftController.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad))
            {
                _leftEventBool = (GameEventBool)LeftEventsDictionnary.Get("LeftThumbTouching");
                _leftEventBool.Raise(true);
            }
            else if (LeftController.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
            {
                _leftEventBool = (GameEventBool)LeftEventsDictionnary.Get("LeftThumbTouching");
                _leftEventBool.Raise(false);
            }
            #endregion TOUCHPAD

            #region GRIP
            temp = LeftVariablesDictionnary.Get("GripIsDown");

            if (!temp.Value && LeftController.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
            {
                temp.SetValue(true);
                _leftEvent = (GameEvent)LeftEventsDictionnary.Get("LeftGripDown");
                _leftEvent.Raise();
            }
            else if (temp.Value && LeftController.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
            {
                temp.SetValue(false);
                _leftEvent = (GameEvent)LeftEventsDictionnary.Get("LeftGripUp");
                _leftEvent.Raise();
            }
            #endregion GRIP

            #region MENU
            temp = LeftVariablesDictionnary.Get("MenuIsDown");

            if (!temp.Value && LeftController.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
            {
                temp.SetValue(true);
                _leftEvent = (GameEvent)LeftEventsDictionnary.Get("LeftMenuDown");
                _leftEvent.Raise();
            }
            else if (temp.Value && LeftController.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu))
            {
                temp.SetValue(false);
                _leftEvent = (GameEvent)LeftEventsDictionnary.Get("LeftMenuUp");
                _leftEvent.Raise();
            }
            #endregion MENU
        }

        /// <summary>
        /// Handle the Right Controller input and put them in the Events
        /// </summary>
        void CheckRightControllerInput()
        {
            BoolVariable temp;

            #region TRIGGER
            temp = RightVariablesDictionnary.Get("TriggerIsDown");

            if (!temp.Value && RightController.GetHairTriggerDown())
            {
                temp.SetValue(true);
                _rightEvent = (GameEvent)RightEventsDictionnary.Get("RightTriggerDown");
                _rightEvent.Raise();
            }
            else if (temp.Value && RightController.GetHairTriggerUp())
            {
                temp.SetValue(false);
                _rightEvent = (GameEvent)RightEventsDictionnary.Get("RightTriggerUp");
                _rightEvent.Raise();
            }
            #endregion TRIGGER

            #region TOUCHPAD
            temp = RightVariablesDictionnary.Get("ThumbIsDown");

            RightThumbOrientation.SetValue(RightController.GetAxis());

            if (RightController.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
            {
                temp.SetValue(true);
                _rightEvent = (GameEvent)RightEventsDictionnary.Get("RightThumbDown");
                _rightEvent.Raise();
            }
            else if (temp.Value && !RightController.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
            {
                temp.SetValue(false);
                _rightEvent = (GameEvent)RightEventsDictionnary.Get("RightThumbUp");
                _rightEvent.Raise();
            }

            if (RightController.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad))
            {
                _leftEventBool = (GameEventBool)RightEventsDictionnary.Get("RightThumbTouching");
                _leftEventBool.Raise(true);
            }
            else if (RightController.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
            {
                _leftEventBool = (GameEventBool)RightEventsDictionnary.Get("RightThumbTouching");
                _leftEventBool.Raise(false);
            }
            #endregion TOUCHPAD

            #region GRIP
            temp = RightVariablesDictionnary.Get("GripIsDown");

            if (!temp.Value && RightController.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
            {
                temp.SetValue(true);
                _rightEvent = (GameEvent)RightEventsDictionnary.Get("RightGripDown");
                _rightEvent.Raise();
            }
            else if (temp.Value && RightController.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
            {
                temp.SetValue(false);
                _rightEvent = (GameEvent)RightEventsDictionnary.Get("RightGripUp");
                _rightEvent.Raise();
            }
            #endregion GRIP

            #region MENU
            temp = RightVariablesDictionnary.Get("MenuIsDown");

            if (!temp.Value && RightController.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
            {
                temp.SetValue(true);
                _rightEvent = (GameEvent)RightEventsDictionnary.Get("RightMenuDown");
                _rightEvent.Raise();
            }
            else if (temp.Value && RightController.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu))
            {
                temp.SetValue(false);
                _rightEvent = (GameEvent)RightEventsDictionnary.Get("RightMenuUp");
                _rightEvent.Raise();
            }
            #endregion MENU
        }
        #endregion PRIVATE_METHODS
    }
}
