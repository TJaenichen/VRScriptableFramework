using Framework.Events;
using Framework.Variables;
using Framework.VR.Controllers;
using Framework.VR.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.VR.UI
{
    /// <summary>
    /// Script placed on the SetupVR Prefab.
    /// Handle all the click from the button linked that are made on colliders.
    /// </summary>
    public class OnColliderClickedHandler : MonoBehaviour
    {
        #region PUBLIC_VARIABLES
        [Header("BoolVariable to check if the Click Button is down, set in the VR Inputs")]
        public BoolVariable RightClickDown;
        public BoolVariable LeftClickDown;
        public BoolVariable GazeClickDown;

        [Header("BoolVariable to verify if something is Hit")]
        public BoolVariable HasClickSomethingRight;
        public BoolVariable HasClickSomethingLeft;
        public BoolVariable HasClickSomethingGaze;

        [Header("The GameEventTransforms to raise when an object is hit")]
        public GameEventTransform RightObjectClicked;
        public GameEventTransform LeftObjectClicked;
        public GameEventTransform GazeObjectClicked;
        #endregion PUBLIC_VARIABLES

        #region PRIVATE_VARIABLES
        private PointerRayCast pointerRayCast;
        #endregion PRIVATE_VARIABLES

        #region MONOBEHAVIOUR_METHODS
        // Update is called once per frame
        void Update()
        {
            if (!CheckReferences())
                return;

            CheckResetClick();
            CheckClick();
        }
        #endregion MONOBEHAVIOUR_METHODS

        //EMPTY
        #region PUBLIC_METHODS
        #endregion PUBLIC_METHODS

        #region PRIVATE_METHODS

        /// <summary>
        /// Check if there's 
        /// </summary>
        void CheckResetClick()
        {
            if (!RightClickDown.Value && HasClickSomethingRight.Value)
                HasClickSomethingRight.SetValue(false);

            if (!LeftClickDown.Value && HasClickSomethingLeft.Value)
                HasClickSomethingLeft.SetValue(false);

            if (!GazeClickDown.Value && HasClickSomethingGaze.Value)
                HasClickSomethingGaze.SetValue(false);
        }

        /// <summary>
        /// If the click button was pressed for the right or left controller, or the gaze, set the Scriptable Object that match
        /// </summary>
        void CheckClick()
        {
            if (RightClickDown.Value && !HasClickSomethingRight.Value)
                HandleClick(pointerRayCast.RightHits, HasClickSomethingRight, RightObjectClicked);

            if (LeftClickDown.Value && !HasClickSomethingLeft.Value)
                HandleClick(pointerRayCast.LeftHits, HasClickSomethingLeft, LeftObjectClicked);

            if (GazeClickDown.Value && !HasClickSomethingGaze.Value)
                HandleClick(pointerRayCast.GazeHits, HasClickSomethingGaze, GazeObjectClicked);
        }

        /// <summary>
        /// Handle the raycastHits to check if one object was clicked
        /// </summary>
        /// <param name="hits">The list of RaycastHits to check</param>
        /// <param name="hasClicked">the BoolVariable to set if something got clicked</param>
        /// <param name="objectClicked">The GameEvent to raise with the transform of the hit</param>
        private void HandleClick(List<RaycastHit> hits, BoolVariable hasClicked, GameEventTransform objectClicked)
        {
            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.layer != pointerRayCast.ExclusionLayer)
                {
                    var hitTransform = hit.collider.transform;

                    hasClicked.SetValue(true);
                    
                    objectClicked.Raise(hitTransform);
                    return;
                }
            }
        }

        /// <summary>
        /// Set the references for the pointerRayCast, the LeftHand and the RightHand
        /// </summary>
        /// <returns>True if everything is setup</returns>
        private bool CheckReferences()
        {
            if (SetupVR.ActiveSDK != null && pointerRayCast == null)
                pointerRayCast = SetupVR.ActiveSDK.GetComponent<PointerRayCast>();

            if (pointerRayCast == null)
                return false;

            return true;
        }
        #endregion PRIVATE_METHODS
    }
}