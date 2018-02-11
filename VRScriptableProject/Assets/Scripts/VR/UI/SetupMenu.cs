using UnityEngine;

namespace Framework.VR.UI
{
    /// <summary>
    /// Setup the localPosition, localRotation, localScale and the parent of a menu
    /// </summary>
	public class SetupMenu : MonoBehaviour 
	{
        #region PUBLIC_VARIABLES
        [Header("The LocalPosition depending on the container")]
        public Vector3 LocalPosition;

        [Header("OPTIONAL : The LocalScale of your Canvas")]
        public Vector3 LocalScale;

        [Header("OPTIONAL : The LocalRotation of your Canvas")]
        public Vector3 LocalRotation;

        [Header("If the Canvas follow the user")]
        [Tooltip("Will set the parent of the Canvas as the UIContainer GameObject under CameraRig")]
        public bool FollowUser;
        #endregion

        #region PRIVATE_VARIABLES
        int uiLayer;
        #endregion

        #region MONOBEHAVIOUR_METHODS
        void Start()
        {
            uiLayer = LayerMask.NameToLayer("UI");
        }

        // Update is called once per frame
        void Update () 
		{
            if (SetupVR.ActiveSDK != null)
            {
                SetTransform();
            }
		}
        #endregion

        //EMPTY
        #region PUBLIC_METHODS

        #endregion
        
        #region PRIVATE_METHODS
        /// <summary>
        /// Set the transform of the Canvas depending on the MenuContainer
        /// </summary>
        void SetTransform()
        {
            if (FollowUser)
            {
                Transform uiContainer = null;
                foreach (Transform t in SetupVR.ActiveSDK.GetComponentsInChildren<Transform>())
                {
                    if (t.name == "UIContainer")
                    {
                        uiContainer = t;
                        break;
                    }
                }
                transform.SetParent(uiContainer);
            }

            if (LocalPosition != null)
                transform.localPosition = LocalPosition;

            if (LocalScale != null && LocalScale != Vector3.zero)
                transform.localScale = LocalScale;

            if (LocalRotation != null && LocalRotation != Vector3.zero)
                transform.localRotation = Quaternion.Euler(LocalRotation);

            this.enabled = false;
        }
        #endregion

        //EMPTY
        #region GETTERS_SETTERS

        #endregion
    }
}