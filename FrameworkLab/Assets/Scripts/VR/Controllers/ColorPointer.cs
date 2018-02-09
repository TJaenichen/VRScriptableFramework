using System.Collections.Generic;
using Framework.Variables;
#if UNITY_EDITOR
#endif
using UnityEngine;

namespace Framework.VR
{
    /// <summary>
    /// Script attached to the CameraRig, check the color of the Linerenderer attached to the controllers
    /// </summary>
    public class ColorPointer : MonoBehaviour
    {
        #region PUBLIC_VARIABLES
        //Pointer State, to switch the color
        public enum UIPointerState { On, Off, Selectable }

        [Header("LineRenderer attached to the right and left controllers")]
        public LineRenderer RightHandPointer;
        public LineRenderer LeftHandPointer;

        [Header("Color Material for LineRenderer")]
        public Material MatOn;
        public Material MatOff;
        public Material MatSelectable;

        [Header("BoolVariable to check if the user aim to the UI")]
        public BoolVariable HasHitUiRight;
        public BoolVariable HasHitUiLeft;
        #endregion PUBLIC_VARIABLES

        #region PRIVATE_VARIABLES
        UIPointerState RightState = UIPointerState.On;
        UIPointerState LeftState = UIPointerState.On;

        private PointerRayCast pointerRayCast;
        #endregion PRIVATE_VARIABLES

        #region MONOBEHAVIOUR_METHODS
        private void Start()
        {
            pointerRayCast = GetComponent<PointerRayCast>();
        }

        void Update ()
        {
            if (!HasHitUiRight.Value)
            {
                RightState = CheckPointer(pointerRayCast.RightHits, RightState, RightHandPointer);
            }
            if (!HasHitUiLeft.Value)
            {
                LeftState = CheckPointer(pointerRayCast.LeftHits, LeftState, LeftHandPointer);
            }
            CheckPointerScale();
        }
        #endregion MONOBEHAVIOUR_METHODS

        //EMPTY
        #region PUBLIC_METHODS
        #endregion

        #region PRIVATE_METHODS
        /// <summary>
        /// Check if the pointer is touching the UI
        /// </summary>
        /// <param name="hits">The list of RayCastHit, from the PointerRayCast Script</param>
        /// <param name="pointerState">The current state of the pointer</param>
        /// <param name="pointer">The linerenderer to which the material is attached</param>
        /// <returns>The new state of the pointer</returns>
        private UIPointerState CheckPointer(List<RaycastHit> hits, UIPointerState pointerState, LineRenderer pointer)
        {
            var uiHit = false;

            foreach (var raycastHit in hits)
            {
                if (raycastHit.collider.gameObject.GetComponentInParent<Canvas>() != null)
                {
                    uiHit = true;
                }
            }

            if (uiHit && pointerState != UIPointerState.Selectable)
            {
                pointer.material = MatSelectable;
                return UIPointerState.Selectable;
            }
            else if (!uiHit && pointerState != UIPointerState.On)
            {
                pointer.material = MatOn;
                return  UIPointerState.On;
            }
            return pointerState;
        }

        /// <summary>
        /// Check the scale of the pointer, if the user is going bigger for some reason
        /// transform here is the CameraRig object
        /// </summary>
        private void CheckPointerScale()
        {
            RightHandPointer.startWidth = transform.localScale.x / 100;
            RightHandPointer.endWidth = transform.localScale.x / 100;

            LeftHandPointer.startWidth = transform.localScale.x / 100;
            LeftHandPointer.endWidth = transform.localScale.x / 100;
        }
        #endregion PRIVATE_METHODS
    }
}