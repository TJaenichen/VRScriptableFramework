using UnityEngine;

namespace Absolute.VR
{
    public class CheckRayLength : MonoBehaviour
    {
        public PointerRayCast PointerRayCast;
        void Update()
        {
            var hasHit = false;
            foreach (var hit in PointerRayCast.RightHits)
            {
                if (hit.collider.GetComponent<Canvas>() != null ||
                    hit.collider.GetComponentInParent<Canvas>() != null ||
                    hit.collider.GetComponentInChildren<Canvas>() != null)
                {
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
                PointerRayCast.RightController.GetComponent<LineRenderer>().SetPositions(new Vector3[]
                {
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 1000), 
                });
            }
            hasHit = false;
            foreach (var hit in PointerRayCast.LeftHits)
            {
                if (hit.collider.GetComponent<Canvas>() != null ||
                    hit.collider.GetComponentInParent<Canvas>() != null ||
                    hit.collider.GetComponentInChildren<Canvas>() != null)
                {
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
                PointerRayCast.LeftController.GetComponent<LineRenderer>().SetPositions(new Vector3[]
                {
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 1000),
                });
            }
        }
    }
}