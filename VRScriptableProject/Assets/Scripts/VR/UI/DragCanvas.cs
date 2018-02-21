using Framework.Variables;
using Framework.VR.Controllers;
using Framework.VR.Utils;
using UnityEngine;

namespace Framework.VR.UI
{
    /// <summary>
    /// Allow the user to drag a Canvas. 
    /// To use this feature, you'll need to create a 3D UI like the VRKeyboard or the UIExample prefabs.
    /// On this window, you need then to create a GameObject containing some colliders (Those are the place
    /// where the user can drag the window). 
    /// Finally, the object with the colliders needs a tag named UIBorder.
    /// </summary>
    public class DragCanvas : MonoBehaviour
    {
        #region PUBLIC_VARIABLES
        [Header("BoolVariable to check if the Trigger is down")]
        public BoolVariable RightTriggerDown;
        public BoolVariable LeftTriggerDown;

        [Header("RaycastHitVariable to where the user has clicked")]
        public RaycastHitVariable HitPoint;
        #endregion

        #region PRIVATE_VARIABLES
        private bool _draggingRight;
        private bool _draggingLeft;
        private Vector3 _curDragPos;
        private Vector3 _lastDragPos;
        private GameObject _draggedThing;
        private float _distance;

        private PointerRayCast PointerRayCast;

        private Transform RightHand;
        private Transform LeftHand;

        private Hand hand; 
        #endregion

        #region MONOBEHAVIOUR_METHODS
        // Update is called once per frame
        void Update()
        {
            if (!CheckReferences())
                return;

            CheckDragging();
        }
        #endregion
        
        #region PUBLIC_METHODS
        /// <summary>
        /// Called from the GameEventTransformListener using UIObjectHit
        /// </summary>
        /// <param name="objectHit">The UI object that was hit</param>
        public void CheckObjectHit(Transform objectHit)
        {
            if (objectHit.tag == "UIBorder")
            {
                if (LeftTriggerDown.Value)
                    StartDragging(HitPoint.Value, Hand.LEFT);
                else
                    StartDragging(HitPoint.Value, Hand.RIGHT);
            }
        }
        #endregion
        
        #region PRIVATE_METHODS
        /// <summary>
        /// Check if the user is already dragging a Canvas
        /// </summary>
        /// <returns>true if the user is dragging a canvas</returns>
        void CheckDragging()
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
            }
            if (_draggingRight)
            {
                KeepDragging();
            }
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

            if (hand == Hand.LEFT)
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
            _draggedThing.transform.root.position += delta;

            if (_draggingLeft)
            {
                _draggedThing.transform.root.rotation =
                    Quaternion.LookRotation(_draggedThing.transform.root.position - PointerRayCast.RightPos);
            }
            else
            {
                _draggedThing.transform.root.rotation =
                    Quaternion.LookRotation(_draggedThing.transform.root.position - PointerRayCast.LeftPos);
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