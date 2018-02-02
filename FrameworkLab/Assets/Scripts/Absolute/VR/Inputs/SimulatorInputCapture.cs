using Framework.Events;
using Framework.RuntimeSet;
using Framework.Variables;
using UnityEngine;

namespace Framework.VR.Inputs
{
    /// <summary>
    /// Set the GameEvent depending on the Keyboard and Mouse Inputs
    /// TODO : THE EVENTS FOR THE OCULUS PARTICULARITIES
    /// </summary>
    public class SimulatorInputCapture : MonoBehaviour
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
        #endregion Left_Controller_Variables

        #region Right_Controller_Variables
        GameEvent _rightEvent;
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
            BoolVariable temp;

            //Left Click
            #region TRIGGER
            temp = LeftVariablesDictionnary.Get("TriggerIsDown");

            if (!temp.Value && Input.GetMouseButtonDown(0))
            {
                temp.SetValue(true);
                _leftEvent = (GameEvent)LeftEventsDictionnary.Get("LeftTriggerDown");
                _leftEvent.Raise();
            }
            else if (temp.Value && Input.GetMouseButtonUp(0))
            {
                temp.SetValue(false);
                _leftEvent = (GameEvent)LeftEventsDictionnary.Get("LeftTriggerUp");
                _leftEvent.Raise();
            }
            #endregion TRIGGER

            //W, A, S and D
            #region THUMBSTICK
            temp = LeftVariablesDictionnary.Get("ThumbIsDown");

            //GO UP
            if (Input.GetKeyDown(KeyCode.W))
            {
                temp.SetValue(true);
                LeftThumbOrientation.SetValue(Vector3.forward);
                _leftEvent = (GameEvent)LeftEventsDictionnary.Get("LeftThumbDown");
                _leftEvent.Raise();
            }
            else if (temp.Value && LeftThumbOrientation.Value.Equals(Vector3.forward))
            {
                temp.SetValue(false);
                LeftThumbOrientation.SetValue(Vector3.zero);
                _leftEvent = (GameEvent)RightEventsDictionnary.Get("LeftThumbUp");
                _leftEvent.Raise();
            }

            // GO DOWN
            if (Input.GetKeyDown(KeyCode.S))
            {
                temp.SetValue(true);
                LeftThumbOrientation.SetValue(Vector3.back);
                _leftEvent = (GameEvent)LeftEventsDictionnary.Get("LeftThumbDown");
                _leftEvent.Raise();
            }
            else if (temp.Value && LeftThumbOrientation.Value.Equals(Vector3.back))
            {
                temp.SetValue(false);
                LeftThumbOrientation.SetValue(Vector3.zero);
                _leftEvent = (GameEvent)RightEventsDictionnary.Get("LeftThumbUp");
                _leftEvent.Raise();
            }

            //GO RIGHT
            if (Input.GetKeyDown(KeyCode.D))
            {
                temp.SetValue(true);
                LeftThumbOrientation.SetValue(Vector3.right);
                _leftEvent = (GameEvent)LeftEventsDictionnary.Get("LeftThumbDown");
                _leftEvent.Raise();
            }
            else if (temp.Value && LeftThumbOrientation.Value.Equals(Vector3.right))
            {
                temp.SetValue(false);
                LeftThumbOrientation.SetValue(Vector3.zero);
                _leftEvent = (GameEvent)RightEventsDictionnary.Get("LeftThumbUp");
                _leftEvent.Raise();
            }

            //GO LEFT
            if (Input.GetKeyDown(KeyCode.A))
            {
                temp.SetValue(true);
                LeftThumbOrientation.SetValue(Vector3.left);
                _leftEvent = (GameEvent)LeftEventsDictionnary.Get("LeftThumbDown");
                _leftEvent.Raise();
            }
            else if (temp.Value && LeftThumbOrientation.Value.Equals(Vector3.left))
            {
                temp.SetValue(false);
                LeftThumbOrientation.SetValue(Vector3.zero);
                _leftEvent = (GameEvent)RightEventsDictionnary.Get("LeftThumbUp");
                _leftEvent.Raise();
            }
            #endregion THUMBSTICK
        
            //Left Shift
            #region GRIP
            temp = LeftVariablesDictionnary.Get("GripIsDown");

            if (!temp.Value && Input.GetKeyDown(KeyCode.LeftShift))
            {
                temp.SetValue(true);
                _leftEvent = (GameEvent)LeftEventsDictionnary.Get("LeftGripDown");
                _leftEvent.Raise();
            }
            else if (temp.Value && Input.GetKeyUp(KeyCode.LeftShift))
            {
                temp.SetValue(false);
                _leftEvent = (GameEvent)LeftEventsDictionnary.Get("LeftGripUp");
                _leftEvent.Raise();
            }
            #endregion GRIP

            //Left Control
            #region MENU
            temp = LeftVariablesDictionnary.Get("MenuIsDown");

            if (!temp.Value && Input.GetKeyDown(KeyCode.LeftControl))
            {
                temp.SetValue(true);
                _leftEvent = (GameEvent)LeftEventsDictionnary.Get("LeftMenuDown");
                _leftEvent.Raise();
            }
            else if (temp.Value && Input.GetKeyUp(KeyCode.LeftControl))
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

            //Right Click
            #region TRIGGER
            temp = RightVariablesDictionnary.Get("TriggerIsDown");

            if (!temp.Value && Input.GetMouseButtonDown(1))
            {
                temp.SetValue(true);
                _rightEvent = (GameEvent)RightEventsDictionnary.Get("RightTriggerDown");
                _rightEvent.Raise();
            }
            else if (temp.Value && Input.GetMouseButtonUp(1))
            {
                temp.SetValue(false);
                _rightEvent = (GameEvent)RightEventsDictionnary.Get("RightTriggerUp");
                _rightEvent.Raise();
            }
            #endregion TRIGGER
        
            //Up, Down, Left and Right Arrows
            #region THUMB
            temp = RightVariablesDictionnary.Get("ThumbIsDown");

            //GO UP
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                RightThumbOrientation.SetValue(Vector3.forward);
                temp.SetValue(true);
                _rightEvent = (GameEvent)RightEventsDictionnary.Get("RightThumbDown");
                _rightEvent.Raise();
            }
            else if (temp.Value && RightThumbOrientation.Value.Equals(Vector3.forward))
            {
                RightThumbOrientation.SetValue(Vector3.zero);
                temp.SetValue(false);
                _rightEvent = (GameEvent)RightEventsDictionnary.Get("RightThumbUp");
                _rightEvent.Raise();
            }

            //GO DOWN
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                RightThumbOrientation.SetValue(Vector3.back);
                temp.SetValue(true);
                _rightEvent = (GameEvent)RightEventsDictionnary.Get("RightThumbDown");
                _rightEvent.Raise();
            }
            else if (temp.Value && RightThumbOrientation.Value.Equals(Vector3.back))
            {
                RightThumbOrientation.SetValue(Vector3.zero);
                temp.SetValue(false);
                _rightEvent = (GameEvent)RightEventsDictionnary.Get("RightThumbUp");
                _rightEvent.Raise();
            }

            //GO RIGHT
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                RightThumbOrientation.SetValue(Vector3.right);
                temp.SetValue(true);
                _rightEvent = (GameEvent)RightEventsDictionnary.Get("RightThumbDown");
                _rightEvent.Raise();
            }
            else if (temp.Value && RightThumbOrientation.Value.Equals(Vector3.right))
            {
                RightThumbOrientation.SetValue(Vector3.zero);
                temp.SetValue(false);
                _rightEvent = (GameEvent)RightEventsDictionnary.Get("RightThumbUp");
                _rightEvent.Raise();
            }

            //GO LEFT
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                RightThumbOrientation.SetValue(Vector3.left);
                temp.SetValue(true);
                _rightEvent = (GameEvent)RightEventsDictionnary.Get("RightThumbDown");
                _rightEvent.Raise();
            }
            else if (temp.Value && RightThumbOrientation.Value.Equals(Vector3.left))
            {
                RightThumbOrientation.SetValue(Vector3.zero);
                temp.SetValue(false);
                _rightEvent = (GameEvent)RightEventsDictionnary.Get("RightThumbUp");
                _rightEvent.Raise();
            }
            #endregion THUMB

            //Right Shift
            #region GRIP
            temp = RightVariablesDictionnary.Get("GripIsDown");

            if (!temp.Value && Input.GetKeyDown(KeyCode.RightShift))
            {
                temp.SetValue(true);
                _rightEvent = (GameEvent)RightEventsDictionnary.Get("RightGripDown");
                _rightEvent.Raise();
            }
            else if (temp.Value && Input.GetKeyUp(KeyCode.RightShift))
            {
                temp.SetValue(false);
                _rightEvent = (GameEvent)RightEventsDictionnary.Get("RightGripUp");
                _rightEvent.Raise();
            }
            #endregion GRIP

            //Right Control
            #region MENU
            temp = RightVariablesDictionnary.Get("MenuIsDown");

            if (!temp.Value && Input.GetKeyDown(KeyCode.RightControl))
            {
                temp.SetValue(true);
                _rightEvent = (GameEvent)RightEventsDictionnary.Get("RightMenuDown");
                _rightEvent.Raise();
            }
            else if (temp.Value && Input.GetKeyUp(KeyCode.RightControl))
            {
                temp.SetValue(false);
                _rightEvent = (GameEvent)RightEventsDictionnary.Get("RightMenuUp");
                _rightEvent.Raise();
            }
            #endregion MENU
        }
        #endregion
    }
}