using UnityEngine;
using System;
using Absolute.VR;
using Framework.Events;
using Framework.Variables;

namespace Framework.VR
{
    public class FlyAndTeleport : MonoBehaviour
    {
        #region PUBLIC_VARIABLES
        [Header("Teleport Settings")]
        public float MinAvatarPositionY = -5;
        public float MaxAvatarPositionY = 780;
        public float AccelerationFactorOnStart = 5.0f;

        [Header("Flying Mode Settings")]
        public float FlightVelocity = 1.0f;
        public float FlightScaleSensitivity = 1.0f;
        public float GroundVelocity = 5f;

        [Header("OPTIONAL : If no UI is used, just set it to constant false")]
        [Tooltip("BoolVariable to set if you use a UI, to check if the right ray has hit a Canvas.")]
        public BoolReference HasHitUiRight;
        [Tooltip("BoolVariable to set if you use a UI, to check if the left ray has hit a Canvas.")]
        public BoolReference HasHitUiLeft;
        #endregion

        #region PRIVATE_VARIABLES
        private GameObject AvatarObject;
        private PointerRayCast pointerRaycast;

        private bool _FlyForward = true;
        private Vector3 _FlightDirection;
        private float _CurrentFlightVelocity = 0.0f;
        private bool _WantToFly = false;

        private float timeSinceStartFlying = 0.0f;
        private float oldTimer = 0.0f;

        private RaycastHit hit;
        private LayerMask GroundLayer;
        private GameEventListenerVector3 rightThumbPosition;
        private bool pressingTouchpad;
        #endregion

        #region MONOBEHAVIOUR_METHODS
        // Use this for initialization
        void Start()
        {
            AvatarObject = SetupVR.ActiveSDK;
            pointerRaycast = AvatarObject.GetComponent<PointerRayCast>();
            AvatarObject.transform.localScale = Vector3.one;
            GroundLayer = LayerMask.NameToLayer("ground");
            rightThumbPosition = GetComponent<GameEventListenerVector3>();
        }

        private void Update()
        {
            CheckInput();

            if (_WantToFly)
            {
                if (timeSinceStartFlying < 1 && timeSinceStartFlying >= 0)
                    timeSinceStartFlying += (Time.deltaTime / AccelerationFactorOnStart);

                if (oldTimer > 0)
                {
                    timeSinceStartFlying = oldTimer;
                    oldTimer = 0;
                }

                _CurrentFlightVelocity = FlightVelocity * timeSinceStartFlying;
            }
            else
            {
                if (oldTimer != 0)
                    oldTimer -= (Time.deltaTime / AccelerationFactorOnStart);
            }

            Fly();
        }
        #endregion

        #region PUBLIC_METHODS
        /// <summary>
        /// Method called when teleport button is clicked
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
            if (rightThumbPosition.vectorValue != Vector3.zero && pressingTouchpad)
            {
                CheckAvatarHeight();
                CalculateFlyForward();
            }
            //if the user was pressing the touchpad and he just stopped to do that
            else if (_WantToFly)
            {
                StopMoving();
            }
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
            _FlyForward = (rightThumbPosition.vectorValue.y >= 0.0f) ? true : false;
            if (!_WantToFly)
            {
                timeSinceStartFlying = 0.0f;
                _WantToFly = true;
            }
        }

        private void SetFlyDirection()
        {
            if (_WantToFly)
            {
                float dir = _FlyForward ? 1.0f : -1.0f;
                _FlightDirection = dir * transform.forward;
            }
        }

        private void Fly()
        {
            if (!_WantToFly)
            {
                if (_CurrentFlightVelocity < 0.001f)
                {
                    return;
                }
                //Sliding effect when touchpad is released
                _CurrentFlightVelocity = Mathf.Clamp(_CurrentFlightVelocity - Time.deltaTime, 0.0f, 10.0f);
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

            // Scale the avatar
            float mappedAngle = MapRangeClamped(angleInDegrees, 60.0f, 120.0f, -1.0f, 1.0f) * FlightScaleSensitivity * 0.002f * scaleTimesdeltaTIme;

            float newAvatarPositionY = AvatarObject.transform.localPosition.y - mappedAngle * MaxAvatarPositionY;
            newAvatarPositionY = Mathf.Clamp(newAvatarPositionY, MinAvatarPositionY, MaxAvatarPositionY);

            //To avoid getting blocked on top of the map
            if (newAvatarPositionY > MaxAvatarPositionY)
            {
                SetYPositionAvatar(MaxAvatarPositionY - 5);
                return;
            }
            else if (newAvatarPositionY < MinAvatarPositionY)
            {
                SetYPositionAvatar(MinAvatarPositionY + 1);
                return;
            }

            SetYPositionAvatar(newAvatarPositionY);
        }

        private float MapRangeClamped(float val, float srcMin, float srcMax, float dstMin, float dstMax)
        {
            if (val >= srcMax) return dstMax;
            if (val <= srcMin) return dstMin;
            return dstMin + (val - srcMin) / (srcMax - srcMin) * (dstMax - dstMin);
        }

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

        private void CheckAvatarHeight()
        {
            var height = AvatarObject.transform.localPosition;
            var scale = AvatarObject.transform.localScale;

            if (height.y + 5 > MaxAvatarPositionY)
                AvatarObject.transform.localPosition = new Vector3(height.x, height.y - 10, height.z);
        }
        #endregion

        #region GETTERS_SETTERS
        public bool PressingTouchpad
        {
            get
            {
                return pressingTouchpad;
            }

            set
            {
                pressingTouchpad = value;
            }
        }

        public bool IsInNormalMode
        {
            get
            {
                return isInNormalMode;
            }

            set
            {
                isInNormalMode = value;
            }
        }
        #endregion
    }
}