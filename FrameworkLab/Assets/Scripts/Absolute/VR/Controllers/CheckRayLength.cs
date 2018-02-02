using UnityEngine;

namespace Framework.VR
{
    /// <summary>
    /// Check if the PointerRayCast has hit something.
    /// </summary>
    public class CheckRayLength : MonoBehaviour
    {
        #region PUBLIC_VARIABLE
        public PointerRayCast PointerRayCast;
        public float MaxLengthLineRenderer = 1000.0f;
        #endregion PUBLIC_VARIABLE

        //EMPTY
        #region PRIVATE_VARIABLE
        #endregion PRIVATE_VARIABLE

        #region MONOBEHAVIOUR_METHODS
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
        void CheckRightController()
        {
            var hasHit = false;

            foreach (var hit in PointerRayCast.RightHits)
            {
                if (hit.collider.GetComponent<Canvas>() != null ||
                    hit.collider.GetComponentInParent<Canvas>() != null ||
                    hit.collider.GetComponentInChildren<Canvas>() != null)
                {
                    //Reduce lineRenderer from the controllers position to the object that was hit
                    PointerRayCast.RightController.GetComponent<LineRenderer>().SetPositions(new Vector3[]
                    {
                        new Vector3(0, 0, 0),
                        PointerRayCast.RightController.transform.InverseTransformPoint(hit.point),
                    });

                    hasHit = true;
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
                if (hit.collider.GetComponent<Canvas>() != null ||
                    hit.collider.GetComponentInParent<Canvas>() != null ||
                    hit.collider.GetComponentInChildren<Canvas>() != null)
                {
                    //Reduce lineRenderer from the controllers position to the object that was hit
                    PointerRayCast.LeftController.GetComponent<LineRenderer>().SetPositions(new Vector3[]
                    {
                        new Vector3(0, 0, 0),
                        PointerRayCast.LeftController.transform.InverseTransformPoint(hit.point),
                    });

                    hasHit = true;
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