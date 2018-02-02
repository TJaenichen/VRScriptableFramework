using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Absolute.VR
{
    public class PointerRayCast : MonoBehaviour
    {
        public GameObject RightController;
        public GameObject LeftController;

        public List<RaycastHit> RightHits = new List<RaycastHit>();
        public List<RaycastHit> LeftHits = new List<RaycastHit>();

        public Ray LeftRay;
        public Ray RightRay;

        public Vector3 RightPos;
        public Vector3 LeftPos;

        void Update ()
        {
            if (SetupVR.SDKLoaded.Contains("Simulator"))
                CheckMouseRays();
            else
                CheckVRRays();
        }

        void CheckVRRays()
        {
            RightPos = RightController.transform.position;
            RightRay = new Ray(RightPos, RightController.transform.TransformDirection(Vector3.forward));
            RightHits = Physics.RaycastAll(RightRay).OrderBy(x => x.distance).ToList();

            LeftPos = LeftController.transform.position;
            LeftRay = new Ray(LeftPos, LeftController.transform.TransformDirection(Vector3.forward));
            LeftHits = Physics.RaycastAll(LeftRay).OrderBy(x => x.distance).ToList();
        }

        void CheckMouseRays()
        {
            RightPos = RightController.transform.position;
            RightRay = new Ray(RightPos, RightController.transform.TransformDirection(Vector3.forward));
            RightHits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition)).OrderBy(x => x.distance).ToList();

            LeftPos = LeftController.transform.position;
            LeftRay = new Ray(LeftPos, LeftController.transform.TransformDirection(Vector3.forward));
            LeftHits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition)).OrderBy(x => x.distance).ToList();
        }
    }
}
