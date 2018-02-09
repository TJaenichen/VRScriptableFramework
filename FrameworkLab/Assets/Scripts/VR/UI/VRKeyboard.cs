using UnityEngine;
using UnityEngine.UI;

namespace Absolute.VR
{
    public class VRKeyboard : MonoBehaviour
    {
        public static VRKeyboard instance;

        public InputField input;

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

        private void Start()
        {
            instance = this;

            input = GetComponentInChildren<InputField>();

            Button [] buttons = GetComponentsInChildren<Button>();
            foreach (Button button in buttons)
            {
                button.onClick.AddListener(delegate { ClickKey(button.name); });
            }
        }
    }
}