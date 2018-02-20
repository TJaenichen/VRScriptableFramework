using UnityEngine;
using UnityEngine.UI;
using Framework.VR.Utils;

namespace Framework.VR.UI
{
    /// <summary>
    /// Script placed on the VRKeyboard Prefab. 
    /// It handle the different keys clicked on the VRKeyboard when an InputField is selected.
    /// </summary>
    public class VRKeyboard : MonoBehaviour
    {
        #region PUBLIC_VARIABLES
        #endregion PUBLIC_VARIABLES

        #region PRIVATE_VARIABLES
        [Tooltip("This can be set via a script by referencing the VRKeyboard")]
        private InputField inputField;

        private PointerRayCast PointerRayCast;

        private Transform RightHand;
        private Transform LeftHand;
        #endregion PRIVATE_VARIABLES

        #region MONOBEHAVIOUR_METHODS
        private void Start()
        {
            Button[] buttons = GetComponentsInChildren<Button>();
            foreach (Button button in buttons)
            {
                button.onClick.AddListener(delegate { ClickKey(button.name); });
            }
        }

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
        /// </summary>
        /// <param name="objectHit">The UI object that was hit</param>
        public void CheckObjectHit(Transform objectHit)
        {
            if (objectHit.tag == "KeyboardKey")
                ClickKey(objectHit.transform.name);
        }
        #endregion PUBLIC_METHODS

        #region PRIVATE_METHODS
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
        
        /// <summary>
        /// Called when a key on the keyboard is clicked
        /// </summary>
        /// <param name="character">The character that was clicked</param>
        private void ClickKey(string character)
        {
            if (!inputField) return;

            switch (character)
            {
                case "Backspace":
                    Backspace();
                    break;
                case "Enter":
                    Enter();
                    break;
                case "Space":
                    inputField.text += " ";
                    break;
                default:
                    inputField.text += character;
                    break;
            }
        }

        /// <summary>
        /// Handle the Backspace Key
        /// </summary>
        private void Backspace()
        {
            if (inputField.text.Length > 0)
            {
                inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
            }
        }

        /// <summary>
        /// Handle the Enter Key
        /// </summary>
        private void Enter()
        {
            Debug.Log("You've typed [" + inputField.text + "]");
        }
        #endregion PRIVATE_METHODS

        #region GETTERS_SETTERS
        public InputField InputField
        {
            get
            {
                return inputField;
            }

            set
            {
                inputField = value;
            }
        }
        #endregion GETTERS_SETTERS
    }
}