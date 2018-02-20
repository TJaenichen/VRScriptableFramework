using Framework.Events;
using Framework.Variables;
using Framework.VR.Controllers;
using Framework.VR.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.VR.UI
{
    /// <summary>
    /// Script placed on the SetupVR Prefab.
    /// Handle all the click with the trigger that are made on the VR UI.
    /// </summary>
    public class UIClickedHandler : MonoBehaviour
    {
        #region PUBLIC_VARIABLES
        [Header("BoolReference to check if the Trigger is down")]
        public BoolVariable RightTriggerDown;
        public BoolVariable LeftTriggerDown;

        [Header("BoolReference to verify if the UI is Hit")]
        public BoolVariable HasHitUiRight;
        public BoolVariable HasHitUiLeft;
        
        [Header("The GameEventTransform to raise when an object is hit")]
        public GameEventTransform ObjectHit;

        [Header("RaycastHitVariable to where the user has clicked")]
        public RaycastHitVariable HitPoint;
        #endregion PUBLIC_VARIABLES

        #region PRIVATE_VARIABLES
        private int uiLayer;

        private PointerRayCast PointerRayCast;

        private Transform RightHand;
        private Transform LeftHand;
        #endregion PRIVATE_VARIABLES

        #region MONOBEHAVIOUR_METHODS
        private void Start()
        {
            uiLayer = LayerMask.NameToLayer("UI");
        }

        // Update is called once per frame
        void Update()
        {
            if (!CheckReferences())
                return;

            if (!RightTriggerDown.Value && HasHitUiRight.Value)
            {
                HasHitUiRight.SetValue(false);
            }
            if (!LeftTriggerDown.Value && HasHitUiLeft.Value)
            {
                HasHitUiLeft.SetValue(false);
            }

            if (RightTriggerDown.Value && !HasHitUiRight.Value)
            {
                Debug.DrawRay(RightHand.transform.position, RightHand.transform.TransformDirection(Vector3.forward),
                              Color.green, 5);
                HandleHits(PointerRayCast.RightHits, Hand.RIGHT);
            }
            if (LeftTriggerDown.Value && !HasHitUiLeft.Value)
            {
                Debug.DrawRay(LeftHand.transform.position, LeftHand.transform.TransformDirection(Vector3.forward),
                              Color.yellow, 5);
                HandleHits(PointerRayCast.LeftHits, Hand.LEFT);
            }
        }
        #endregion MONOBEHAVIOUR_METHODS

        //EMPTY
        #region PUBLIC_METHODS
        #endregion PUBLIC_METHODS

        #region PRIVATE_METHODS
        /// <summary>
        /// Set the references for the pointerRayCast, the LeftHand and the RightHand
        /// </summary>
        /// <returns>True if everything is setup</returns>
        private bool CheckReferences()
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

        /// <summary>
        /// Handle the raycastHits to check if one of them touch the UI
        /// </summary>
        /// <param name="hits">The list hits link to the hand</param>
        /// <param name="hand">The hand to test</param>
        private void HandleHits(List<RaycastHit> hits, Hand hand)
        {
            foreach (var raycastHit in hits)
            {
                var transformHit = raycastHit.collider.transform;
                if (transformHit.gameObject.layer == uiLayer)
                {
                    SetUiHandHit(hand);

                    HitPoint.SetValue(raycastHit);
                    ObjectHit.Raise(transformHit);
                    return;
                }
            }
        }

        /// <summary>
        /// Set the BoolVariable that is dragging the canvas
        /// </summary>
        /// <param name="hand">The hand that press the trigger</param>
        void SetUiHandHit(Hand hand)
        {
            if (hand == Hand.LEFT)
                HasHitUiLeft.SetValue(true);
            else
                HasHitUiRight.SetValue(true);
        }
        #endregion PRIVATE_METHODS
    }
}