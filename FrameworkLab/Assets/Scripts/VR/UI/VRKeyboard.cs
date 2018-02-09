using UnityEngine;
using UnityEngine.UI;

namespace Absolute.VR
{
    public class VRKeyboard : MonoBehaviour
    {
        #region PUBLIC_VARIABLES
        public InputField input;
        #endregion PUBLIC_VARIABLES

        //EMPTY
        #region PRIVATE_VARIABLES
        #endregion PRIVATE_VARIABLES

        #region MONOBEHAVIOUR_METHODS
        private void Start()
        {
            input = GetComponentInChildren<InputField>();

            Button[] buttons = GetComponentsInChildren<Button>();
            foreach (Button button in buttons)
            {
                button.onClick.AddListener(delegate { ClickKey(button.name); });
            }
        }

        void Update()
        {
            
        }
        #endregion MONOBEHAVIOUR_METHODS

        #region PUBLIC_METHODS
        public void ClickKey(string character)
        {
            if (!input) return;

            switch (character)
            {
                case "Backspace":
                    Backspace();
                    break;
                case "Enter":
                    Enter();
                    break;
                case "Space":
                    input.text += " ";
                    break;
                default:
                    input.text += character;
                    break;
            }
        }

        public void Backspace()
        {
            if (input.text.Length > 0)
            {
                input.text = input.text.Substring(0, input.text.Length - 1);
            
                //TODO implement the caret
                //input.caretPosition
                //input.caretWidth
            }
        }

        public void Enter()
        {
            Debug.Log("You've typed [" + input.text + "]");
        }
        #endregion PUBLIC_METHODS

        #region PRIVATE_METHODS
        
        #endregion PRIVATE_METHODS
    }
}