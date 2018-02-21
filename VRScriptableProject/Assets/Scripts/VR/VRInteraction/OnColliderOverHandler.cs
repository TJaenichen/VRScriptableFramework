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
    /// Check if a ray is over a collider.
    /// </summary>
    public class OnColliderOverHandler : MonoBehaviour
    {
        #region PUBLIC_VARIABLES
        [Header("BoolVariable to verify if something is Hit")]
        public BoolVariable IsOverSomethingRight;
        public BoolVariable IsOverSomethingLeft;
        public BoolVariable IsOverSomethingGaze;

        [Header("RaycastHitVariable to where the user has clicked")]
        public RaycastHitVariable RightHitPoint;
        public RaycastHitVariable LeftHitPoint;
        public RaycastHitVariable GazeHitPoint;

        [Header("The GameEventTransforms to raise when an object is hit")]
        public GameEventTransform RightOverObject;
        public GameEventTransform LeftOverObject;
        public GameEventTransform GazeOverObject;
        #endregion PUBLIC_VARIABLES

        #region PRIVATE_VARIABLES
        private PointerRayCast pointerRayCast;
        #endregion PRIVATE_VARIABLES

        #region MONOBEHAVIOUR_METHODS
        // Update is called once per frame
        void Update()
        {
            if (!CheckReferences())
                return;
            
            CheckIsOver();
        }
        #endregion MONOBEHAVIOUR_METHODS

        //EMPTY
        #region PUBLIC_METHODS
        #endregion PUBLIC_METHODS

        #region PRIVATE_METHODS
        /// <summary>
        /// Check if each raycast (Controllers and Gaze) are over something
        /// </summary>
        void CheckIsOver()
        {
            HandleOver(pointerRayCast.RightHits, IsOverSomethingRight, RightHitPoint, RightOverObject);

            HandleOver(pointerRayCast.LeftHits, IsOverSomethingLeft, LeftHitPoint, LeftOverObject);

            HandleOver(pointerRayCast.GazeHits, IsOverSomethingGaze, GazeHitPoint, GazeOverObject);
        }

        /// <summary>
        /// Handle the raycastHits to check if one of them touch something
        /// </summary>
        /// <param name="hits">The list of RaycastHits to check</param>
        /// <param name="isOver">the BoolVariable to set if something got hit</param>
        /// <param name="hitPoint">The Hit Point where the raycast collide</param>
        /// <param name="objectOver">The GameEvent to raise with the transform of the hit</param>
        private void HandleOver(List<RaycastHit> hits, BoolVariable isOver, RaycastHitVariable hitPoint, GameEventTransform objectOver)
        {
            //If nothing is hit, we set the hasHit value to false
            if (hits.Count == 0)
            {
                isOver.SetValue(false);
            }
            else
            {
                foreach (var hit in hits)
                {
                    // If something is hit and is not from the Exclusion layer, we set everything and return
                    if (hit.collider.gameObject.layer != pointerRayCast.ExclusionLayer)
                    {
                        var hitTransform = hit.collider.transform;

                        isOver.SetValue(true);

                        hitPoint.SetValue(hit);
                        objectOver.Raise(hitTransform);
                        return;
                    }
                    //If the only hit was on the exclusion layer, we set the hasHit value to false
                    else
                    {
                        isOver.SetValue(false);
                    }
                }
            }
        }

        /// <summary>
        /// Set the references for the pointerRayCast, the LeftHand and the RightHand
        /// </summary>
        /// <returns>True if everything is setup</returns>
        private bool CheckReferences()
        {
            if (SetupVR.ActiveSDK != null && pointerRayCast == null)
                pointerRayCast = SetupVR.ActiveSDK.GetComponent<PointerRayCast>();

            if (pointerRayCast == null)
                return false;

            return true;
        }
        #endregion PRIVATE_METHODS
    }
}