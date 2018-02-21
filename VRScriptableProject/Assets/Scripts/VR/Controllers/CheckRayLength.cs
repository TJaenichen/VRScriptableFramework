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
        
        [Header("OPTIONAL : Layer to exclude from the raycast.")]
        public LayerMask ExclusionLayer;
        #endregion PUBLIC_VARIABLE

        #region PRIVATE_VARIABLE
        private Utils.PointerRayCast PointerRayCast;
        #endregion PRIVATE_VARIABLE

        #region MONOBEHAVIOUR_METHODS
        void Start()
        {
            PointerRayCast = GetComponent<Utils.PointerRayCast>();
        }

        void Update()
        {
            CheckRightController();
            CheckLeftController();

            if (PointerRayCast.UseGaze && Reticle != null)
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
            foreach (var hit in PointerRayCast.RightHits)
            {
                if (hit.collider.gameObject.layer != ExclusionLayer)
                {
                    //Reduce lineRenderer from the controllers position to the object that was hit
                    PointerRayCast.RightController.GetComponent<LineRenderer>().SetPositions(new Vector3[]
                    {
                        new Vector3(0, 0, 0),
                        PointerRayCast.RightController.transform.InverseTransformPoint(hit.point),
                    });
                }
                
                return;
            }
            
            //put back lineRenderer to its normal length if nothing was hit
            PointerRayCast.RightController.GetComponent<LineRenderer>().SetPositions(new Vector3[]
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
            foreach (var hit in PointerRayCast.LeftHits)
            {
                if (hit.collider.gameObject.layer != ExclusionLayer)
                {
                    //Reduce lineRenderer from the controllers position to the object that was hit
                    PointerRayCast.LeftController.GetComponent<LineRenderer>().SetPositions(new Vector3[]
                    {
                        new Vector3(0, 0, 0),
                        PointerRayCast.LeftController.transform.InverseTransformPoint(hit.point),
                    });
                }

                return;
            }

            //put back lineRenderer to its normal length if nothing was hit
            PointerRayCast.LeftController.GetComponent<LineRenderer>().SetPositions(new Vector3[]
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
            foreach (var hit in PointerRayCast.LeftHits)
            {
                if (hit.collider.gameObject.layer != ExclusionLayer)
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