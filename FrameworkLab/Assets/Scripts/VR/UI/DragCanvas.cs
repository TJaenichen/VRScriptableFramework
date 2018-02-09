using Framework.Variables;
using Framework.VR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Framework.VR.UI
{
    public class DragCanvas : MonoBehaviour
    {
        #region PUBLIC_VARIABLES
        [Header("BoolReference to verify if the UI is Hit")]
        public BoolVariable HasHitUiRight;
        public BoolVariable HasHitUiLeft;

        [Header("BoolReference to check if the Trigger is down")]
        public BoolReference RightTriggerDown;
        public BoolReference LeftTriggerDown;
        #endregion

        #region PRIVATE_VARIABLES
        private bool _draggingRight;
        private bool _draggingLeft;
        private Vector3 _curDragPos;
        private Vector3 _lastDragPos;
        private GameObject _draggedThing;
        private float _distance;
        private int uiLayer;

        private PointerRayCast PointerRayCast;

        private Transform RightHand;
        private Transform LeftHand;

        enum Hand
        {
            Left,
            Right
        }
        #endregion

        #region MONOBEHAVIOUR_METHODS
        // Use this for initialization
        void Start()
        {
            uiLayer = LayerMask.NameToLayer("UI");
        }

        // Update is called once per frame
        void Update()
        {
            if (!CheckReferences())
                return;

            if (!RightTriggerDown.Value && HasHitUiRight)
            {
                HasHitUiRight.SetValue(false);
            }
            if (!LeftTriggerDown.Value && HasHitUiLeft)
            {
                HasHitUiLeft.SetValue(false);
            }

            if (CheckDragging())
            {
                return;
            }

            if (RightTriggerDown.Value && !HasHitUiRight)
            {
                Debug.DrawRay(RightHand.transform.position, RightHand.transform.TransformDirection(Vector3.forward),
                              Color.green, 5);
                HandleHits(PointerRayCast.RightHits, Hand.Right);
            }
            if (LeftTriggerDown.Value && !HasHitUiLeft)
            {
                Debug.DrawRay(LeftHand.transform.position, LeftHand.transform.TransformDirection(Vector3.forward),
                              Color.yellow, 5);
                HandleHits(PointerRayCast.LeftHits, Hand.Left);
            }
        }
        #endregion
        
        //EMPTY
        #region PUBLIC_METHODS

        #endregion
        
        #region PRIVATE_METHODS
        /// <summary>
        /// Check if the user is already dragging a Canvas
        /// </summary>
        /// <returns>true if the user is dragging a canvas</returns>
        bool CheckDragging()
        {
            if (_draggingLeft && !LeftTriggerDown.Value)
            {
                _draggingLeft = false;
            }
            if (_draggingRight && !RightTriggerDown.Value)
            {
                _draggingRight = false;
            }
            
            if (_draggingLeft)
            {
                KeepDragging();
                return true;
            }
            if (_draggingRight)
            {
                KeepDragging();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Set the references when the user start to drag a Canvas
        /// </summary>
        /// <param name="raycastHit">The raycastHit hitting the UI</param>
        /// <param name="hand">The hand with which the user is dragging the Canvas</param>
        void StartDragging(RaycastHit raycastHit, Hand hand)
        {
            if (!(_draggingLeft || _draggingRight))
            {
                _lastDragPos = raycastHit.point;
                _curDragPos = raycastHit.point;
            }

            if (hand == Hand.Left)
            {
                _draggingLeft = true;
                _distance = Vector3.Distance(PointerRayCast.LeftPos, raycastHit.point);
            }
            else
            {
                _draggingRight = true;
                _distance = Vector3.Distance(PointerRayCast.RightPos, raycastHit.point);
            }

            _draggedThing = raycastHit.collider.gameObject;
        }

        /// <summary>
        /// Move the canvas while the user is pressing the trigger
        /// </summary>
        void KeepDragging()
        {
            Ray ray;

            if (_draggingLeft)
                ray = PointerRayCast.LeftRay;
            else
                ray = PointerRayCast.RightRay;

            _lastDragPos = _curDragPos;
            _curDragPos = ray.GetPoint(_distance);
            var delta = _curDragPos - _lastDragPos;
            _draggedThing.transform.parent.parent.transform.position += delta;

            if (_draggingLeft)
            {
                _draggedThing.transform.parent.parent.transform.rotation =
                    Quaternion.LookRotation(_draggedThing.transform.parent.parent.transform.position - PointerRayCast.RightPos);
            }
            else
            {
                _draggedThing.transform.parent.parent.transform.rotation =
                    Quaternion.LookRotation(_draggedThing.transform.parent.parent.transform.position - PointerRayCast.LeftPos);
            }
        }

        /// <summary>
        /// Set the BoolVariable that is dragging the canvas
        /// </summary>
        /// <param name="hand">The hand that press the trigger</param>
        void SetUiHandHit(Hand hand)
        {
            if (hand == Hand.Left)
                HasHitUiLeft.SetValue(true);
            else
                HasHitUiRight.SetValue(true);
        }

        /// <summary>
        /// Handle the raycastHits to check if one of them touch the UI
        /// </summary>
        /// <param name="hits">The list hits link to the hand</param>
        /// <param name="hand">The hand to test</param>
        void HandleHits(List<RaycastHit> hits, Hand hand)
        {
            foreach (var raycastHit in hits)
            {
                var raycastCollider = raycastHit.collider;
                if (raycastCollider.gameObject.layer == uiLayer)
                {
                    SetUiHandHit(hand);

                    //When a user click on the border surrounding a canvas
                    if (raycastCollider.gameObject.name == "Border")
                        StartDragging(raycastHit, hand);
                }
            }
        }

        /// <summary>
        /// Set the references for the pointerRayCast, the LeftHand and the RightHand
        /// </summary>
        /// <returns>True if everything is setup</returns>
        bool CheckReferences()
        {
            if (RightHand == null)
                RightHand = SetupVR.RightControllerTransform;

            if (LeftHand == null)
                LeftHand = SetupVR.LeftControllerTransform;

            if (SetupVR.ActiveSDK != null && PointerRayCast == null)
                PointerRayCast = SetupVR.ActiveSDK.GetComponent<PointerRayCast>();

            if (PointerRayCast == null || LeftHand == null || RightHand == null)
                return false;

            return true;
        } 
        #endregion
        
        #region GETTERS_SETTERS
        #endregion
    }
}