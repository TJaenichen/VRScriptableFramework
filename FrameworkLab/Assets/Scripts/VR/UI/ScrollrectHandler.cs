using Framework.Variables;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.VR.UI
{
    /// <summary>
    /// Script to attach to the GameObject containing the scrollbar and the viewport
    /// This GameObject must also contains box colliders for the zone where the user is allow to
    /// grab and scroll the UI.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class ScrollrectHandler : MonoBehaviour
    {
        #region PUBLIC_VARIABLES
        [Header("Interaction Parameters")]
        [Tooltip("The threshold for the difference between two hit points. This difference is calculated on each frame Update.")]
        public float ThresholdDifference = 0.005f;
        [Tooltip("The speed at which the view is scrolling.")]
        public float Speed = 0.015f;

        [Header("BoolVariable for the Triggers")]
        public BoolVariable LeftTriggerDown;
        public BoolVariable RightTriggerDown;
        #endregion PUBLIC_VARIABLES

        #region PRIVATE_VARIABLES
        bool controllerAreSetup;

        float oldYPosition = 0.0f;

        [Tooltip("The pointerRayCast script containing the raycast from the two controllers.")]
        PointerRayCast pointerRayCast;
        [Tooltip("The scrollbar to move, child of the gameobject to which this script is attached to.")]
        Scrollbar scrollbar;
        [Tooltip("The colliders to check for the raycast, attach to this gameobject. You can add as many colliders you want.")]
        BoxCollider[] boxs;
        #endregion PRIVATE_VARIABLES

        #region MONOBEHAVIOUR_METHODS
        private void Start()
        {
            scrollbar = GetComponentInChildren<Scrollbar>();
            boxs = GetComponents<BoxCollider>();
        }

        private void Update()
        {
            if (!controllerAreSetup)
            {
                pointerRayCast = SetupVR.ActiveSDK.GetComponent<PointerRayCast>();
                controllerAreSetup = true;
                return;
            }

            if (LeftTriggerDown.Value)
                CheckRayCast(pointerRayCast.LeftHits);

            if (RightTriggerDown.Value)
                CheckRayCast(pointerRayCast.RightHits);
        }
        #endregion MONOBEHAVIOUR_METHODS

        //EMPTY
        #region PUBLIC_METHODS
        #endregion PUBLIC_METHODS

        #region PRIVATE_METHODS
        /// <summary>
        /// Check if a raycastHit as touched one of the box collider
        /// </summary>
        /// <param name="hits">The list of RaycastHit to check</param>
        void CheckRayCast(List<RaycastHit> hits)
        {
            foreach (RaycastHit hit in hits)
            {
                foreach (BoxCollider box in boxs)
                {
                    if (hit.collider == box)
                    {
                        Scroll(hit.point.y);
                    }
                }
            }
        }

        /// <summary>
        /// Method to scroll the Viewport
        /// </summary>
        /// <param name="newPos">the point to which the raycastHit is</param>
        void Scroll(float newPos)
        {
            var difference = newPos - oldYPosition;

            if (difference > ThresholdDifference)
                scrollbar.value += Speed;
            else if (difference < -ThresholdDifference)
                scrollbar.value -= Speed;

            oldYPosition = newPos;
        }
        #endregion PRIVATE_METHODS

        #region GETTERS_SETTERS
        #endregion GETTERS_SETTERS
    }
}