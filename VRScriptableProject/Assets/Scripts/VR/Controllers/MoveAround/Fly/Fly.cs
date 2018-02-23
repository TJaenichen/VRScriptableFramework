using UnityEngine;
using Framework.Variables;
using Framework.VR.Controllers;
using Framework.VR.Utils;

namespace Framework.VR.MoveAround
{
    /// <summary>
    /// Allow the user to fly with the thumb and teleport with the grip. 
    /// Place this script on a Controller Script, and set the public variable to use it.
    /// 
    /// To use the flying mode, a Vector3Reference link to the thumb position on the touchpad 
    /// and a BoolReference to check if the user is pressing the touchpad are necessary. Those are set
    /// in the InputCapture script (this one is normally placed on the first GameObject of the SDK prefab).
    /// </summary>
    public class Fly : MonoBehaviour
    {
        #region PUBLIC_VARIABLES

        #region Scriptable_Objects_Variable
        [Header("The Scriptable Object containing the flying parameters")]
        public FlyingParametersVariable MovementParameters;

        [Header("VR Framework Variables")]
        [Tooltip("Those Values are set in the InputCapture script on the first Gameobject of the SDK.\n" +
            "You can find them in the folder Assets/Variables/VR.")]
        public Vector3Variable RightThumbPosition;
        public Vector3Variable LeftThumbPosition;
        [Tooltip("Those Values are set in the InputCapture script on the first Gameobject of the SDK.\n" +
            "You can find them in the folder Assets/Variables/VR.")]
        public BoolVariable RightTouchpadIsPressed;
        public BoolVariable LeftTouchpadIsPressed;
        #endregion Scriptable_Objects_Variable

        [Header("The hand this script is attached to.")]
        public Hand Hand;
        #endregion

        #region PRIVATE_VARIABLES
        private BoolVariable touchpadIsPressed;
        private Vector3Variable thumbPosition;

        private GameObject avatarObject;                //The CameraRig object

        private bool _flyForward = true;
        private Vector3 _flightDirection;
        private float _currentFlightVelocity = 0.0f;
        private bool _wantToFly = false;

        private float timeSinceStartFlying = 0.0f;
        private float oldTimer = 0.0f;
        #endregion

        #region MONOBEHAVIOUR_METHODS
        // Use this for initialization
        void Start()
        {
            avatarObject = SetupVR.ActiveSDK;
            avatarObject.transform.localScale = Vector3.one;
            if (SetupVR.DeviceLoaded != Device.OVR)
                MovementParameters.RotateCamera = false;

            CheckHand();
        }

        private void Update()
        {
            CheckInput();
            CheckFlyingMode();
        }
        #endregion

        //EMPTY
        #region PUBLIC_METHODS
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
            else if (_wantToFly)
            {
                StopMoving();
            }
            else if (MovementParameters.RotateCamera)
            {
                Rotate();
            }
        }

        /// <summary>
        /// Add an acceleration sensation when the user is flying
        /// </summary>
        private void CheckFlyingMode()
        {
            //If the user is pressing the flying button
            if (_wantToFly)
            {
                if (timeSinceStartFlying >= 0 && timeSinceStartFlying < 1)
                    timeSinceStartFlying += (Time.deltaTime / MovementParameters.SlidingEffectFactor);

                if (oldTimer > 0)
                {
                    timeSinceStartFlying = oldTimer;
                    oldTimer = 0;
                }

                _currentFlightVelocity = MovementParameters.BasicFlightVelocityFactor * timeSinceStartFlying;
            }
            else
            {
                if (oldTimer != 0)
                    oldTimer -= (Time.deltaTime / MovementParameters.SlidingEffectFactor);
            }

            FlyAway();
        }

        /// <summary>
        /// Called when user release thumstick/touchpad
        /// </summary>
        private void StopMoving()
        {
            oldTimer = timeSinceStartFlying;
            _wantToFly = false;
        }

        /// <summary>
        /// Calculate if the user is flying forward or backward.
        /// </summary>
        /// <param name="pressingTouchpad">If the user press the thumbstick/touchpad</param>
        private void CalculateFlyForward()
        {
            _flyForward = (thumbPosition.Value.y >= 0.0f) ? true : false;
            if (!_wantToFly)
            {
                timeSinceStartFlying = 0.0f;
                _wantToFly = true;
            }
        }

        /// <summary>
        /// Check if the user fly forward or backward
        /// </summary>
        private void SetFlyDirection()
        {
            if (_wantToFly)
            {
                float dir = _flyForward ? 1.0f : -1.0f;
                _flightDirection = dir * transform.forward;
            }
        }

        /// <summary>
        /// Actual script to make the user fly
        /// </summary>
        private void FlyAway()
        {
            if (!_wantToFly)
            {
                if (_currentFlightVelocity < 0.001f)
                {
                    return;
                }
                //Sliding effect when touchpad is released
                _currentFlightVelocity = Mathf.Clamp(_currentFlightVelocity - Time.deltaTime, 0.0f, MovementParameters.MaxFlightSpeed);
            }

            SetFlyDirection();

            float scaleTimesdeltaTIme = Mathf.Abs(avatarObject.transform.localScale.x * Time.deltaTime * _currentFlightVelocity);
            float angleInDegrees = Mathf.Acos(Vector3.Dot(_flightDirection, Vector3.up)) * 360.0f / Mathf.PI;
            angleInDegrees -= 90.0f;

            //Move the avatar
            Vector3 movementOnGroundPlane = _flightDirection;
            movementOnGroundPlane.y = 0.0f;
            movementOnGroundPlane.Normalize();
            
            avatarObject.transform.localPosition += _currentFlightVelocity * scaleTimesdeltaTIme * movementOnGroundPlane;

            float mappedAngle = MapRangeClamped(angleInDegrees, 60.0f, 120.0f, -1.0f, 1.0f) * MovementParameters.GetVerticalSpeed() * scaleTimesdeltaTIme;
            float newAvatarPositionY = avatarObject.transform.localPosition.y - mappedAngle * MovementParameters.MaxAvatarPositionY;

            newAvatarPositionY = Mathf.Clamp(newAvatarPositionY, MovementParameters.MinAvatarPositionY, MovementParameters.MaxAvatarPositionY);

            SetYPositionAvatar(newAvatarPositionY);
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
        /// Set the position of the Avatar on the Y axis
        /// </summary>
        /// <param name="newYPosition"></param>
        private void SetYPositionAvatar(float newYPosition)
        {
            //Set the height of the avatar to the same values as the scale
            avatarObject.transform.localPosition = new Vector3
                (
                avatarObject.transform.localPosition.x,
                newYPosition,
                avatarObject.transform.localPosition.z
                );
        }

        /// <summary>
        /// Only for the Oculus, as the sensors are not following the controllers if the user is in front of them
        /// </summary>
        void Rotate()
        {
            var x = thumbPosition.Value.x;
            if (x > 0.5f || x < -0.5f)
            {
                avatarObject.transform.Rotate(new Vector3(0, x, 0) * 2);
            }
        }

        /// <summary>
        /// Assign either the Left or Right variable depending on the Hand that is selected
        /// </summary>
        void CheckHand()
        {
            if (Hand == Hand.LEFT)
            {
                thumbPosition = LeftThumbPosition;
                touchpadIsPressed = LeftTouchpadIsPressed;
            }
            else
            {
                thumbPosition = RightThumbPosition;
                touchpadIsPressed = RightTouchpadIsPressed;
            }
        }
        #endregion

        #region GETTERS_SETTERS
        #endregion
    }
}