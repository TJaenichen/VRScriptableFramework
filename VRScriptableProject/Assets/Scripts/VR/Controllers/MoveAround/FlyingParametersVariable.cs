using UnityEngine;

namespace Framework.VR.MoveAround
{
    /// <summary>
    /// Contain all the parameters for the Flying Mode
    /// </summary>
    [CreateAssetMenu(menuName = "Variables/VR/Flying Parameters")]
    public class FlyingParametersVariable : ScriptableObject
    {
        [Multiline]
        public string DeveloperDescription = "";
        
        [Tooltip("The maximum speed at which the user can go in the X and Z axis.")]
        public float MaxFlightSpeed = 8.0f;

        [Tooltip("The basic speed factor for the X and Z axis.")]
        public float BasicFlightVelocityFactor = 1.0f;

        [Tooltip("The speed factor for the speed in the X and Z axis.")]
        public float YAxisSpeed = 0.02f;

        [Tooltip("The minimun local position at which the user can go in the Y axis.")]
        public float MinAvatarPositionY = -5.0f;

        [Tooltip("The maximum local position at which the user can go in the Y axis.")]
        public float MaxAvatarPositionY = 1000.0f;

        [Tooltip("The factor for the acceleration and deceleration effect. Set to 0 to remove this effect.")]
        public float SlidingEffectFactor = 5.0f;

        [Tooltip("Only relevant for Oculus, as sensors can lose track of the Controllers when the user is not facing them.")]
        public bool RotateCamera = false;
	}
}