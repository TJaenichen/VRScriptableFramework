using Framework.Variables;
using Framework.VR.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.VR.UI.Example
{
    /// <summary>
    /// Script attached to the UIExample Prefabs.
    /// It's a quick example on how to interact with the Scriptable UI System in VR.
    /// </summary>
    public class ClickHandlerUIExample : MonoBehaviour
    {
        #region PUBLIC_VARIABLES
        [Header("BoolReference to check if the Trigger is down")]
        public BoolVariable RightTriggerDown;
        public BoolVariable LeftTriggerDown;

        [Header("RaycastHitReference to where the user has clicked")]
        public RaycastHitVariable HitPoint;

        [Header("The VRKeyboard in the Scene")]
        public GameObject VRKeyboard;
        #endregion PUBLIC_VARIABLES

        #region PRIVATE_VARIABLES
        private PointerRayCast PointerRayCast;

        private Transform RightHand;
        private Transform LeftHand;
        #endregion PRIVATE_VARIABLES

        #region MONOBEHAVIOUR_METHODS
        // Update is called once per frame
        void Update()
        {
            if (!CheckReferences())
                return;
        }
        #endregion MONOBEHAVIOUR_METHODS

        #region PUBLIC_METHODS
        /// <summary>
        /// Called from the GameEventTransformListener using UIObjectHit
        /// THIS SCRIPT CAN ONLY BE PLACED ONE TIME, AS WE'RE COMPARING TAGS
        /// YOU CANNOT PUT THIS SCRIPT ON MULTIPLE UIs, OR YOU NEED TO CHECK REFERENCES THAT ARE NOT TAGS
        /// </summary>
        /// <param name="objectHit">The UI object that was hit</param>
        public void CheckObjectHit(Transform objectHit)
        {
            switch (objectHit.tag)
            {
                case ("Toggle"):
                    HandleToggle(objectHit.gameObject);
                    break;
                case ("Button"):
                    HandleButton(objectHit.gameObject);
                    break;
                case ("InputField"):
                    HandleInputField(objectHit.gameObject);
                    break;
            }
        }
        #endregion PUBLIC_METHODS

        #region PRIVATE_METHODS
        /// <summary>
        /// Handle the toggle example when it's clicked
        /// </summary>
        /// <param name="toggleHit">The gameObject that was hit</param>
        void HandleToggle(GameObject toggleHit)
        {
            toggleHit.GetComponent<Toggle>().isOn = !toggleHit.GetComponent<Toggle>().isOn;
        }

        /// <summary>
        /// Handle the button when it's clicked
        /// </summary>
        /// <param name="buttonHit">The gameObject that was hit</param>
        void HandleButton(GameObject buttonHit)
        {
            Text t = buttonHit.GetComponentInChildren<Text>();
            t.fontSize = 12; 
            t.text = "You're a wonderfull person and I love you";
        }

        /// <summary>
        /// Handle the inputField when it's clicked
        /// </summary>
        /// <param name="inputFieldHit">The gameObject that was hit</param>
        void HandleInputField(GameObject inputFieldHit)
        {
            var inputField = inputFieldHit.GetComponent<InputField>();
            foreach (Text t in inputField.GetComponentsInChildren<Text>())
            {
                t.text = "";
            }
            inputField.ActivateInputField();

            if (VRKeyboard != null)
                VRKeyboard.GetComponent<VRKeyboard>().InputField = inputField;
        }

        /// <summary>
        /// Set the references for the pointerRayCast, the LeftHand and the RightHand
        /// </summary>
        /// <returns>True if everything is setup</returns>
        private bool CheckReferences()
        {
            if (RightHand == null)
                RightHand = SetupVR.RightControllerTransform;

            if (LeftHand == null)
                LeftHand = SetupVR.LeftControllerTransform;

            if (SetupVR.ActiveSDK != null && PointerRayCast == null)
                PointerRayCast = SetupVR.ActiveSDK.GetComponent<PointerRayCast>();

            if (PointerRayCast == null || LeftHand == null || RightHand == null)
                return false;

            return true;
        }
        #endregion PRIVATE_METHODS
    }
}