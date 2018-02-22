using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.VR.Gaze
{
    /// <summary>
    /// Contain all the parameters for the Gaze Reticle 
    /// </summary>
    [CreateAssetMenu(menuName = "Variables/VR/Reticle Parameters")]
    public class ReticleParametersVariable : ScriptableObject
    {
        #region PUBLIC_VARIABLES

        [Multiline]
        public string DeveloperDescription = "";

        [Tooltip("The default distance away from the camera the reticle is placed.")]
        public float m_DefaultDistance = 200.0f;

        [Tooltip("Whether the reticle should be placed parallel to a surface.")]
        public bool m_UseNormal;

        #endregion PUBLIC_VARIABLES


        #region GETTERS_SETTERS

        public bool UseNormal
        {
            get { return m_UseNormal; }
            set { m_UseNormal = value; }
        }

        #endregion GETTERS_SETTERS
    }
}