using Framework.VR.Utils;
using System;
using UnityEngine;

// This code is a refactored copy of the one provided by the Unity's VR Sample.
// This script need to be placed on the CameraRig object.
namespace Framework.VR.Gaze
{
	public class VREyeRaycaster : MonoBehaviour 
	{
        #region PUBLIC_VARIABLES
        public event Action<RaycastHit> OnRaycasthit;                   // This event is called every frame that the user's gaze is over a collider.
        #endregion


        #region PRIVATE_VARIABLES

        #region SERIALIZED_FIELDS
        [Header("VREyeRaycaster Parameters")]
        [Tooltip("The camera from where the raycast start.")]
        [SerializeField] private Transform m_Camera;

        [Tooltip("Layers to exclude from the raycast.")]
        [SerializeField] private LayerMask m_ExclusionLayers;

        [Tooltip("The reticle, if applicable.")]
        [SerializeField] private Reticle m_Reticle;

        [Tooltip("Optionally show the debug ray.")]
        [SerializeField] private bool m_ShowDebugRay;

        [Tooltip("Debug ray length.")]
        [SerializeField] private float m_DebugRayLength = 5f;

        [Tooltip("How long the Debug ray will remain visible.")]
        [SerializeField]
        private float m_DebugRayDuration = 1f;

        [Tooltip("How far into the scene the ray is cast.")]
        [SerializeField] private float m_RayLength = 500f;
        #endregion SERIALIZED_FIELDS

        #region NON_SERIALIZED_FIELDS
        private VRInteractiveItem m_CurrentInteractible;                //The current interactive item
        private VRInteractiveItem m_LastInteractible;                   //The last interactive item
        #endregion NON_SERIALIZED_FIELDS

        #endregion PRIVATE_VARIABLES


        #region MONOBEHAVIOUR_METHODS

        private void Update()
        {
            EyeRaycast();
        }

        #endregion


        #region PUBLIC_METHODS

        #endregion


        #region PRIVATE_METHODS

        /// <summary>
        /// Handle the Hits of the eye Raycasts
        /// </summary>
        private void EyeRaycast()
        {
            // Show the debug ray if required
            if (m_ShowDebugRay)
            {
                Debug.DrawRay(m_Camera.position, m_Camera.forward * m_DebugRayLength, Color.blue, m_DebugRayDuration);
            }

            // Create a ray that points forwards from the camera.
            Ray ray = new Ray(m_Camera.position, m_Camera.forward);
            RaycastHit hit;

            // Do the raycast forwards to see if we hit an interactive item
            if (Physics.Raycast(ray, out hit, m_RayLength, ~m_ExclusionLayers))
            {
                VRInteractiveItem interactible = hit.collider.GetComponent<VRInteractiveItem>(); //attempt to get the VRInteractiveItem on the hit object
                m_CurrentInteractible = interactible;

                // If we hit an interactive item and it's not the same as the last interactive item, then call Over
                if (interactible && interactible != m_LastInteractible)
                    interactible.Over();

                // Deactive the last interactive item 
                if (interactible != m_LastInteractible)
                    DeactiveLastInteractible();

                m_LastInteractible = interactible;

                // Something was hit, set at the hit position.
                if (m_Reticle)
                    m_Reticle.SetPosition(hit);

                if (OnRaycasthit != null)
                    OnRaycasthit(hit);
            }
            else
            {
                // Nothing was hit, deactive the last interactive item.
                DeactiveLastInteractible();
                m_CurrentInteractible = null;

                // Position the reticle at default distance.
                if (m_Reticle)
                    m_Reticle.SetPositionToNormal();
            }
        }

        /// <summary>
        /// Deactivate the last interactible object that was selected
        /// </summary>
        private void DeactiveLastInteractible()
        {
            if (m_LastInteractible == null)
                return;

            m_LastInteractible.Out();
            m_LastInteractible = null;
        }


        private void HandleUp()
        {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.Up();
        }


        private void HandleDown()
        {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.Down();
        }


        private void HandleClick()
        {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.Click();
        }


        private void HandleDoubleClick()
        {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.DoubleClick();

        }

        #endregion


        #region GETTERS_SETTERS

        #endregion
    }
}