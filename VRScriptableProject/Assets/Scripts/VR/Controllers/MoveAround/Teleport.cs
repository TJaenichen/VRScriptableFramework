﻿using Framework.Variables;
using Framework.VR.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.VR.MoveAround
{
    /// <summary>
    /// To use the Teleport feature, add a BoxCollider and a Layer named "ground" on your Ground Object.
    /// Create a GameEventListener for the button you want to use as Teleport button, and link it to
    /// the GameEvent concerned in the Event folder.
    /// </summary>
    public class Teleport : MonoBehaviour 
	{
        #region PUBLIC_VARIABLES
        [Header("BoolVariable to check if the user aim to the UI")]
        public BoolVariable HasHitUiRight;
        public BoolVariable HasHitUiLeft;

        [Header("The Hand this script is assign to")]
        public Hand hand;
        #endregion


        #region PRIVATE_VARIABLES
        private LayerMask groundLayer;                  // The Layer for the Ground
        private GameObject avatarObject;                //The CameraRig object
        private List<RaycastHit> raycastHits;
        #endregion


        #region MONOBEHAVIOUR_METHODS
        // Use this for initialization
        void Start ()
        {
            groundLayer = LayerMask.NameToLayer("Ground");
            avatarObject = SetupVR.ActiveSDK;
        }

        void Update()
        {
            if (avatarObject == null) 
            {
                avatarObject = SetupVR.ActiveSDK;
                return;
            }
        }
        #endregion


        #region PUBLIC_METHODS
        /// <summary>
        /// Method to call from the GameEventListener, linked to the button Event used to teleport.
        /// </summary>
        public void OnClickHandler()
        {
            raycastHits = GetRaycastHits();
            foreach (RaycastHit hit in raycastHits)
            {
                Debug.Log(hit.collider.gameObject);
                Debug.Log(hit.collider.gameObject.layer);
                if (hit.collider.gameObject.layer == groundLayer.value)
                {
                    avatarObject.transform.position = new Vector3(hit.point.x, avatarObject.transform.position.y,
                        hit.point.z);
                    return;
                }
            }
        }
        #endregion


        #region PRIVATE_METHODS
        /// <summary>
        /// Chose which RaycastHits list to use depending on the Hand on which this script is attached to
        /// </summary>
        /// <returns>The Left or Right RaycastHit List from the PointerRayCast Script</returns>
        List<RaycastHit> GetRaycastHits()
        {
            if (hand == Hand.LEFT)
                return avatarObject.GetComponent<PointerRayCast>().LeftHits;
            else
                return avatarObject.GetComponent<PointerRayCast>().RightHits;
        }

        bool CheckReferences()
        {
            if (avatarObject != null)
                return true;

            groundLayer = LayerMask.NameToLayer("Ground");
            Debug.Log("Return False");
            return false;
        }
        #endregion

        //EMPTY
        #region GETTERS_SETTERS

        #endregion
    }
}