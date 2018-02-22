using UnityEngine;
using UnityEngine.UI;

// This code is a refactored copy of the one provided by the Unity's VR Sample.
namespace Framework.VR.Gaze
{
    /// <summary>
    /// The reticle is a small point at the centre of the screen.
    /// It is used as a visual aid for aiming.The position of the
    /// reticle is either at a default position in space or on the
    /// surface of a VRInteractiveItem as determined by the VREyeRaycaster.
    /// </summary>
    public class Reticle : MonoBehaviour
    {
        #region PUBLIC_VARIABLES
        [Header("The Scriptable Object containing the reticle parameters")]
        public ReticleParametersVariable ReticleVariables;

        [Header("References linked to SDK")]
        [Tooltip("We need to affect the reticle's transform.")]
        public Transform m_ReticleTransform;

        [Tooltip("The reticle is always placed relative to the camera.")]
        public Transform m_Camera;

        [Tooltip("Reference to the image component that represents the reticle.")]
        public Image m_Image;
        #endregion PUBLIC_VARIABLES

        #region PRIVATE_VARIABLES

        private Vector3 m_OriginalScale;        // Since the scale of the reticle changes, the original scale needs to be stored.
        private Quaternion m_OriginalRotation;  // Used to store the original rotation of the reticle.

        #endregion PRIVATE_VARIABLES

        #region MONOBEHAVIOUR_METHODS

        private void Awake()
        {
            // Store the original scale and rotation.
            m_OriginalScale = m_ReticleTransform.localScale;
            m_OriginalRotation = m_ReticleTransform.localRotation;
        }

        #endregion MONOBEHAVIOUR_METHODS

        #region PUBLIC_METHODS
        /// <summary>
        /// Hide the Reticle
        /// </summary>
        public void Hide()
        {
            m_Image.enabled = false;
        }
        
        /// <summary>
        /// Show the Reticle
        /// </summary>
        public void Show()
        {
            m_Image.enabled = true;
        }
        
        /// <summary>
        /// This method is called when the reticle didn't hit anything.
        /// It set it back to the "normal" position.
        /// </summary>
        public void SetPositionToNormal()
        {
            // Set the position of the reticle to the default distance in front of the camera.
            m_ReticleTransform.position = 
                m_Camera.position + m_Camera.forward * ReticleVariables.m_DefaultDistance;

            // Set the scale based on the original and the distance from the camera.
            m_ReticleTransform.localScale = m_OriginalScale * ReticleVariables.m_DefaultDistance;

            // The rotation should just be the default.
            m_ReticleTransform.localRotation = m_OriginalRotation;
        }


        /// <summary>
        /// This overload of SetPosition is used when the Gaze Raycast has hit something.
        /// </summary>
        public void SetPosition(RaycastHit hit)
        {
            m_ReticleTransform.position = hit.point;
            m_ReticleTransform.localScale = m_OriginalScale * hit.distance;

            // If the reticle should use the normal of what has been hit...
            if (ReticleVariables.m_UseNormal)
                // ... set it's rotation based on it's forward vector facing along the normal.
                m_ReticleTransform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
            else
                // However if it isn't using the normal then it's local rotation should be as it was originally.
                m_ReticleTransform.localRotation = m_OriginalRotation;
        }

        #endregion PUBLIC_METHODS
        
        #region GETTERS_SETTERS
        public Transform ReticleTransform { get { return m_ReticleTransform; } }
        #endregion GETTERS_SETTERS
    }
}