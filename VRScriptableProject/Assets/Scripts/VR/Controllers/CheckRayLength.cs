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
        #endregion PUBLIC_VARIABLE
        
        #region PRIVATE_VARIABLE
        private PointerRayCast PointerRayCast;
        private LayerMask uiLayer;
        #endregion PRIVATE_VARIABLE

        #region MONOBEHAVIOUR_METHODS
        void Start()
        {
            uiLayer = LayerMask.NameToLayer("UI");
            PointerRayCast = GetComponent<PointerRayCast>();
        }

        void Update()
        {
            CheckRightController();
            CheckLeftController();
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
            var hasHit = false;

            foreach (var hit in PointerRayCast.RightHits)
            {
                //If the collider is a UI element
                if (hit.collider != null)
                {
                    //Reduce lineRenderer from the controllers position to the object that was hit
                    PointerRayCast.RightController.GetComponent<LineRenderer>().SetPositions(new Vector3[]
                    {
                        new Vector3(0, 0, 0),
                        PointerRayCast.RightController.transform.InverseTransformPoint(hit.point),
                    });

                    hasHit = true;
                    break;
                }
            }

            if (!hasHit)
            {
                //put back lineRenderer to its normal length
                PointerRayCast.RightController.GetComponent<LineRenderer>().SetPositions(new Vector3[]
                {
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, MaxLengthLineRenderer),
                });
            }
        }

        /// <summary>
        /// Check if the left ray has hit something on the way
        /// </summary>
        void CheckLeftController()
        {
            var hasHit = false;

            foreach (var hit in PointerRayCast.LeftHits)
            {
                //If the collider is not a UI element
                if (hit.collider != null)
                {
                    //Reduce lineRenderer from the controllers position to the object that was hit
                    PointerRayCast.LeftController.GetComponent<LineRenderer>().SetPositions(new Vector3[]
                    {
                        new Vector3(0, 0, 0),
                        PointerRayCast.LeftController.transform.InverseTransformPoint(hit.point),
                    });

                    hasHit = true;
                    break;
                }
            }

            if (!hasHit)
            {
                //put back lineRenderer to its normal length
                PointerRayCast.LeftController.GetComponent<LineRenderer>().SetPositions(new Vector3[]
                {
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, MaxLengthLineRenderer),
                });
            }
        }
        #endregion PRIVATE_METHODS
    }
}