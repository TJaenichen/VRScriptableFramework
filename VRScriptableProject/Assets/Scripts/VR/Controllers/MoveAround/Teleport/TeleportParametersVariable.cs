using UnityEngine;

namespace Framework.VR.MoveAround
{
    /// <summary>
    /// Contain all the parameters for the Teleport Script
    /// </summary>
    [CreateAssetMenu(menuName = "Variables/VR/Teleport Parameters")]
    public class TeleportParametersVariable : ScriptableObject
    {
        [Multiline]
        public string DeveloperDescription = "";
        
        [Header("Height Adjusting Parameters")]
        [Tooltip("If you want to adjust the height to the point that was hit.")]
        public bool AdjustHeight = false;
        [Tooltip("The height at which the user is teleported above the ground.")]
        public float HeightAboveGround = 1.5f;
    }
}