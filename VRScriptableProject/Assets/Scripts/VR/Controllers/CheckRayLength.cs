using Framework.VR.Gaze;
using UnityEngine;

namespace Framework.VR
{
    /// <summary>
    /// Script attached to the CameraRig of each VR SDK Prefab.
    /// Check if the PointerRayCast has hit something.
    /// </summary>
    public class CheckRayLength : MonoBehaviour
    {
        #region PUBLIC_VARIABLE
        [Header("The maximum length of the Ray pointer")]
        public float MaxLengthLineRenderer = 1000.0f;
        
        [Header("OPTIONAL : The reticle for the Gaze.")]
        public Reticle Reticle;
        #endregion PUBLIC_VARIABLE

        #region PRIVATE_VARIABLE
        private Utils.PointerRayCast pointerRayCast;
        #endregion PRIVATE_VARIABLE

        #region MONOBEHAVIOUR_METHODS
        void Start()
        {
            pointerRayCast = GetComponent<Utils.PointerRayCast>();
        }

        void Update()
        {
            CheckRightController();
            CheckLeftController();

            if (pointerRayCast.UseGaze && Reticle != null)
                CheckGaze();
        }
        #endregion MONOBEHAVIOUR_METHODS

        //EMPTY
        #region PUBLIC_METHODS
        #endregion PUBLIC_METHODS

        #region PRIVATE_METHODS
        /// <summary>
        /// Check if the right ray has hit something on the way
        /// </summary>
        void CheckRightController()
        {
            foreach (var hit in pointerRayCast.RightHits)
            {
                if (hit.collider.gameObject.layer != pointerRayCast.ExclusionLayer)
                {
                    //Reduce lineRenderer from the controllers position to the object that was hit
                    pointerRayCast.RightController.GetComponent<LineRenderer>().SetPositions(new Vector3[]
                    {
                        new Vector3(0, 0, 0),
                        pointerRayCast.RightController.transform.InverseTransformPoint(hit.point),
                    });
                }
                
                return;
            }
            
            //put back lineRenderer to its normal length if nothing was hit
            pointerRayCast.RightController.GetComponent<LineRenderer>().SetPositions(new Vector3[]
            {
                new Vector3(0, 0, 0),
                new Vector3(0, 0, MaxLengthLineRenderer),
            });
        }

        /// <summary>
        /// Check if the left ray has hit something on the way
        /// </summary>
        void CheckLeftController()
        {
            foreach (var hit in pointerRayCast.LeftHits)
            {
                if (hit.collider.gameObject.layer != pointerRayCast.ExclusionLayer)
                {
                    //Reduce lineRenderer from the controllers position to the object that was hit
                    pointerRayCast.LeftController.GetComponent<LineRenderer>().SetPositions(new Vector3[]
                    {
                        new Vector3(0, 0, 0),
                        pointerRayCast.LeftController.transform.InverseTransformPoint(hit.point),
                    });
                }

                return;
            }

            //put back lineRenderer to its normal length if nothing was hit
            pointerRayCast.LeftController.GetComponent<LineRenderer>().SetPositions(new Vector3[]
            {
                new Vector3(0, 0, 0),
                new Vector3(0, 0, MaxLengthLineRenderer),
            });
        }

        /// <summary>
        /// Check if the Gaze ray has hit something on the way
        /// </summary>
        void CheckGaze()
        {
            foreach (var hit in pointerRayCast.GazeHits)
            {
                Debug.Log(hit.collider);
                if (hit.collider.gameObject.layer != pointerRayCast.ExclusionLayer)
                {
                    //Reduce the reticle positon to the object that was hit
                    Reticle.SetPosition(hit);
                    return;
                }
            }

            //put back the reticle positon to its normal distance if nothing was hit
            Reticle.SetPositionToNormal();
        }
        #endregion PRIVATE_METHODS
    }
}