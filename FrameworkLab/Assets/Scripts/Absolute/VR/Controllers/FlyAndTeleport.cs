using UnityEngine;
using Absolute.VR;
using Framework.Variables;
using Framework.Events;

namespace Framework.VR
{
    /// <summary>
    /// Allow the user to fly with the thumb and teleport with the grip. 
    /// Place this script on a Controller Script, and set the public variable to use it.
    /// 
    /// To use the flying mode, a Vector3Reference link to the thumb position on the touchpad 
    /// and a BoolReference to check if the user is pressing the touchpad are necessary. Those are set
    /// in the InputCapture script (this one is normally placed on the first GameObject of the SDK prefab).
    /// </summary>
    [RequireComponent(typeof(Vector3Reference), typeof(BoolReference))]
    public class FlyAndTeleport : MonoBehaviour
    {
        #region PUBLIC_VARIABLES
        [Header("Flying Mode Settings")]
        public bool UseFlyingMode = false;
        public float MaxFlightSpeed = 8.0f;
        public float AccelerationFactorOnStart = 5.0f;
        public float MinAvatarPositionY = -5;
        public float MaxAvatarPositionY = 780;
        [Tooltip("This Reference is set in the InputCapture script on the first Gameobject of the SDK.\n" +
            "You can find them in the folder Assets/Variables/VR.")]
        public Vector3Reference thumbPosition;
        [Tooltip("This Reference is set in the InputCapture script on the first Gameobject of the SDK.\n" +
            "You can find them in the folder Assets/Variables/VR.")]
        public BoolReference touchpadIsPressed;

        [Header("BoolReference to set if you use a UI")]
        [Tooltip("If no UI is used, just set it to constant false.")]
        public BoolReference HasHitUiRight;
        [Tooltip("If no UI is used, just set it to constant false.")]
        public BoolReference HasHitUiLeft;
        #endregion

        #region PRIVATE_VARIABLES
        private GameObject AvatarObject;                //The CameraRig object
        private PointerRayCast pointerRaycast;          //The pointerRaycast on the Avatar Object

        private bool _FlyForward = true;
        private Vector3 _FlightDirection;
        private float _CurrentFlightVelocity = 0.0f;
        private bool _WantToFly = false;

        private float timeSinceStartFlying = 0.0f;
        private float oldTimer = 0.0f;

        private RaycastHit hit;
        private LayerMask GroundLayer;
        #endregion

        #region MONOBEHAVIOUR_METHODS
        // Use this for initialization
        void Start()
        {
            AvatarObject = SetupVR.ActiveSDK;
            pointerRaycast = AvatarObject.GetComponent<PointerRayCast>();
            AvatarObject.transform.localScale = Vector3.one;
            GroundLayer = LayerMask.NameToLayer("ground");
        }

        private void Update()
        {
            if (UseFlyingMode)
            {
                CheckInput();
                CheckFlyingMode();
            }
        }
        #endregion

        #region PUBLIC_METHODS
        /// <summary>
        /// To use the Teleport feature, add a collider and a layer named "ground" on your ground plane.
        /// Create a GameEventListener for the button you want to use as Teleport button, and link it to
        /// the GameEvent concerned in the Event folder.
        /// </summary>
        public void Teleport()
        {
            if (HasHitUiRight.Value || HasHitUiLeft.Value)
            {
                return;
            }

            foreach (RaycastHit hit in pointerRaycast.RightHits)
            {
                if (hit.collider.gameObject.layer == GroundLayer.value)
                {
                    AvatarObject.transform.position = new Vector3(hit.point.x, AvatarObject.transform.position.y,
                        hit.point.z);
                    return;
                }
            }
        }
        #endregion

        #region PRIVATE_METHODS
        /// <summary>
        /// Check the input on the controllers, mostly for flying mode
        /// </summary>
        private void CheckInput()
        {
            //Right thumbstick is moved. ActivateFlyingMode is set in editor with the RightThumbMove GameEventVector3.
            if (touchpadIsPressed.Value)
            {
                CalculateFlyForward();
            }
            //if the user was pressing the touchpad and he just stopped to do that
            else if (_WantToFly)
            {
                StopMoving();
            }
        }

        /// <summary>
        /// Add an acceleration sensation when the user is flying
        /// </summary>
        private void CheckFlyingMode()
        {
            //If the user is pressing the flying button
            if (_WantToFly)
            {
                if (timeSinceStartFlying >= 0 && timeSinceStartFlying < 1)
                    timeSinceStartFlying += (Time.deltaTime / AccelerationFactorOnStart);

                if (oldTimer > 0)
                {
                    timeSinceStartFlying = oldTimer;
                    oldTimer = 0;
                }
            }
            else
            {
                if (oldTimer != 0)
                    oldTimer -= (Time.deltaTime / AccelerationFactorOnStart);
            }

            Fly();
        }

        /// <summary>
        /// Called when user release thumstick/touchpad
        /// </summary>
        private void StopMoving()
        {
            oldTimer = timeSinceStartFlying;
            _WantToFly = false;
        }

        /// <summary>
        /// Calculate if the user is flying forward or backward.
        /// </summary>
        /// <param name="pressingTouchpad">If the user press the thumbstick/touchpad</param>
        private void CalculateFlyForward()
        {
            _FlyForward = (thumbPosition.Value.y >= 0.0f) ? true : false;
            if (!_WantToFly)
            {
                timeSinceStartFlying = 0.0f;
                _WantToFly = true;
            }
        }

        /// <summary>
        /// Check if the user fly forward or backward
        /// </summary>
        private void SetFlyDirection()
        {
            if (_WantToFly)
            {
                float dir = _FlyForward ? 1.0f : -1.0f;
                _FlightDirection = dir * transform.forward;
            }
        }

        /// <summary>
        /// Actual script to make the user fly
        /// </summary>
        private void Fly()
        {
            if (!_WantToFly)
            {
                if (_CurrentFlightVelocity < 0.001f)
                {
                    return;
                }
                //Sliding effect when touchpad is released
                _CurrentFlightVelocity = Mathf.Clamp(timeSinceStartFlying - Time.deltaTime, 0.0f, MaxFlightSpeed);
            }

            SetFlyDirection();

            float scaleTimesdeltaTIme = Mathf.Abs(AvatarObject.transform.localScale.x * Time.deltaTime * _CurrentFlightVelocity);
            float angleInDegrees = Mathf.Acos(Vector3.Dot(_FlightDirection, Vector3.up)) * 360.0f / Mathf.PI;
            angleInDegrees -= 90.0f;

            //Move the avatar
            Vector3 movementOnGroundPlane = _FlightDirection;
            movementOnGroundPlane.y = 0.0f;
            movementOnGroundPlane.Normalize();

            AvatarObject.transform.position += _CurrentFlightVelocity * scaleTimesdeltaTIme * movementOnGroundPlane;
            
            float mappedAngle = MapRangeClamped(angleInDegrees, 60.0f, 120.0f, -1.0f, 1.0f) * 0.002f * scaleTimesdeltaTIme;

            float newAvatarPositionY = AvatarObject.transform.localPosition.y - mappedAngle * MaxAvatarPositionY;
            newAvatarPositionY = Mathf.Clamp(newAvatarPositionY, MinAvatarPositionY, MaxAvatarPositionY);

            CheckAvatarYPosition(newAvatarPositionY);
        }

        /// <summary>
        /// Some magical stuffs
        /// </summary>
        /// <param name="val"></param>
        /// <param name="srcMin"></param>
        /// <param name="srcMax"></param>
        /// <param name="dstMin"></param>
        /// <param name="dstMax"></param>
        /// <returns></returns>
        private float MapRangeClamped(float val, float srcMin, float srcMax, float dstMin, float dstMax)
        {
            if (val >= srcMax) return dstMax;
            if (val <= srcMin) return dstMin;
            return dstMin + (val - srcMin) / (srcMax - srcMin) * (dstMax - dstMin);
        }

        /// <summary>
        /// Check if the Avatar is not too high or to low
        /// </summary>
        /// <param name="newAvatarPositionY">The new position of the avatar</param>
        void CheckAvatarYPosition(float newAvatarPositionY)
        {
            //To avoid getting blocked on top of the map
            if (newAvatarPositionY > MaxAvatarPositionY)
                SetYPositionAvatar(MaxAvatarPositionY - 2);

            //To avoid getting blocked under the map
            else if (newAvatarPositionY < MinAvatarPositionY)
                SetYPositionAvatar(MinAvatarPositionY + 1);

            else
                SetYPositionAvatar(newAvatarPositionY);
        }

        /// <summary>
        /// Set the position of the Avatar on the Y axis
        /// </summary>
        /// <param name="newYPosition"></param>
        private void SetYPositionAvatar(float newYPosition)
        {
            //Set the height of the avatar to the same values as the scale
            AvatarObject.transform.localPosition = new Vector3
                (
                AvatarObject.transform.localPosition.x,
                newYPosition,
                AvatarObject.transform.localPosition.z
                );
        }
        #endregion
        
        #region GETTERS_SETTERS
        #endregion
    }
}