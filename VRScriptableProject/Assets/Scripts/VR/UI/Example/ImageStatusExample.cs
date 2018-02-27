using Framework.VR.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.VR.UI.Example
{
	public class ImageStatusExample : MonoBehaviour 
	{
        // EMPTY
        #region PUBLIC_VARIABLES

        #endregion PUBLIC_VARIABLES

            
        #region PRIVATE_VARIABLES
        bool isSetup;
        PointerRayCast pointerRayCast;
        Collider imageCollider;
        Image image;
        #endregion PRIVATE_VARIABLES


        #region MONOBEHAVIOUR_METHODS
        // Use this for initialization
        void Start () 
		{
			
		}
		
		// Update is called once per frame
		void Update () 
		{
			if (!isSetup)
            {
                CheckReferences();
                return;
            }

            CheckRayCastGaze(pointerRayCast.GazeHits);
		}
	    #endregion MONOBEHAVIOUR_METHODS


	    // EMPTY
	    #region PUBLIC_METHODS

	    #endregion PUBLIC_METHODS

            
	    #region PRIVATE_METHODS
        void CheckRayCastGaze(List<RaycastHit> hits)
        {
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider == imageCollider)
                {
                    image.enabled = true;
                    return;
                }
                else
                    image.enabled = false;
            }
        }

        void CheckReferences()
        {
            try
            {
                pointerRayCast = SetupVR.ActiveSDK.GetComponent<PointerRayCast>();
                imageCollider = GetComponent<Collider>();
                image = GetComponent<Image>();
                isSetup = true;
            } catch
            {
                Debug.Log("CameraRig not on the scene yet.");
            }
        }
        #endregion PRIVATE_METHODS


        // EMPTY
        #region GETTERS_SETTERS

        #endregion GETTERS_SETTERS
    }
}