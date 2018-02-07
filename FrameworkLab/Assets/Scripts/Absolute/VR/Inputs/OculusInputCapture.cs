using Framework.Events;
using Framework.RuntimeSet;
using Framework.Variables;
using UnityEngine;

namespace Framework.VR
{
    /// <summary>
    /// Set the GameEvent depending on the Oculus Inputs
    /// </summary>
    public class OculusInputCapture : MonoBehaviour
    {
        #region PUBLIC_VARIABLES
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
        #endregion

        #region PRIVATE_VARIABLES

        #region Left_Controller_Variables
        GameEvent _leftEvent;
        GameEventBool _leftEventBool;
        #endregion Left_Controller_Variables

        #region Right_Controller_Variables
        GameEvent _rightEvent;
        GameEventBool _rightEventBool;
        #endregion Right_Controller_Variables

        #endregion

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
            BoolVariable temp;

            #region TRIGGER
            temp = LeftVariablesDictionnary.Get("TriggerIsDown");

            if (!temp.Value && OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
            {
                temp.SetValue(true);
                _leftEvent = (GameEvent)LeftEventsDictionnary.Get("LeftTriggerDown");
                _leftEvent.Raise();
            }
            else if (temp.Value && !OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
            {
                temp.SetValue(false);
                _leftEvent = (GameEvent)LeftEventsDictionnary.Get("LeftTriggerUp");
                _leftEvent.Raise();
            }
            #endregion TRIGGER

            #region THUMBSTICK
            temp = LeftVariablesDictionnary.Get("ThumbIsDown");

            LeftThumbOrientation.SetValue(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick));
            
            if (OVRInput.Get(OVRInput.Button.PrimaryThumbstick))
            {
                temp.SetValue(true);
                _leftEvent = (GameEvent)LeftEventsDictionnary.Get("LeftThumbDown");
                _leftEvent.Raise();
            }
            else if (temp.Value && !OVRInput.Get(OVRInput.Button.PrimaryThumbstick))
            {
                temp.SetValue(false);
                _leftEvent = (GameEvent)LeftEventsDictionnary.Get("LeftThumbUp");
                _leftEvent.Raise();
            }

            if (OVRInput.Get(OVRInput.Touch.PrimaryThumbstick))
            {
                _leftEventBool = (GameEventBool)LeftEventsDictionnary.Get("LeftThumbTouching");
                _leftEventBool.Raise(true);
            }
            else if (!OVRInput.Get(OVRInput.Touch.PrimaryThumbstick))
            {
                _leftEventBool = (GameEventBool)LeftEventsDictionnary.Get("LeftThumbTouching");
                _leftEventBool.Raise(false);
            }
            #endregion THUMBSTICK

            #region GRIP
            temp = LeftVariablesDictionnary.Get("GripIsDown");

            if (!temp.Value && OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
            {
                temp.SetValue(true);
                _leftEvent = (GameEvent)LeftEventsDictionnary.Get("LeftGripDown");
                _leftEvent.Raise();
            }
            else if (temp.Value && !OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
            {
                temp.SetValue(true);
                _leftEvent = (GameEvent)LeftEventsDictionnary.Get("LeftGripUp");
                _leftEvent.Raise();
            }
            #endregion GRIP

            #region MENU
            temp = LeftVariablesDictionnary.Get("MenuIsDown");

            if (!temp.Value && OVRInput.Get(OVRInput.Button.Start))
            {
                temp.SetValue(true);
                _leftEvent = (GameEvent)LeftEventsDictionnary.Get("LeftMenuDown");
                _leftEvent.Raise();
            }
            else if (temp.Value && !OVRInput.Get(OVRInput.Button.Start))
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

            if (!temp.Value && OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
            {
                temp.SetValue(true);
                _rightEvent = (GameEvent)RightEventsDictionnary.Get("RightTriggerDown");
                _rightEvent.Raise();
            }
            else if (temp.Value && !OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
            {
                temp.SetValue(false);
                _rightEvent = (GameEvent)RightEventsDictionnary.Get("RightTriggerUp");
                _rightEvent.Raise();
            }
            #endregion TRIGGER

            #region THUMBSTICK
            temp = RightVariablesDictionnary.Get("ThumbIsDown");

            RightThumbOrientation.SetValue(OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick));
            
            if (OVRInput.Get(OVRInput.Button.SecondaryThumbstick))
            {
                temp.SetValue(true);
                _rightEvent = (GameEvent)RightEventsDictionnary.Get("RightThumbDown");
                _rightEvent.Raise();
            }
            else if (temp.Value && !OVRInput.Get(OVRInput.Button.SecondaryThumbstick))
            {
                temp.SetValue(false);
                _rightEvent = (GameEvent)RightEventsDictionnary.Get("RightThumbUp");
                _rightEvent.Raise();
            }

            if (OVRInput.Get(OVRInput.Touch.SecondaryThumbstick))
            {
                _rightEventBool = (GameEventBool)RightEventsDictionnary.Get("RightThumbTouching");
                _rightEventBool.Raise(true);
            }
            else if (!OVRInput.Get(OVRInput.Touch.SecondaryThumbstick))
            {
                _rightEventBool = (GameEventBool)RightEventsDictionnary.Get("RightThumbTouching");
                _rightEventBool.Raise(false);
            }
            #endregion THUMBSTICK

            #region GRIP
            temp = RightVariablesDictionnary.Get("GripIsDown");

            if (!temp.Value && OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
            {
                temp.SetValue(true);
                _rightEvent = (GameEvent)RightEventsDictionnary.Get("RightGripDown");
                _rightEvent.Raise();
            }
            else if (temp.Value && !OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
            {
                temp.SetValue(false);
                _rightEvent = (GameEvent)RightEventsDictionnary.Get("RightGripUp");
                _rightEvent.Raise();
            }
            #endregion GRIP

            //No Right menu button on the oculus

            ////Button B    TODO !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
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
}