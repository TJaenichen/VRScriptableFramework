using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Framework.VR
{
    /// <summary>
    /// Check the Raycast of the two controllers and reference them in two list of RaycastHit
    /// </summary>
    public class PointerRayCast : MonoBehaviour
    {
        #region PUBLIC_VARIABLES
        [Header("The two controllers gameObject")]
        public GameObject RightController;
        public GameObject LeftController;

        [Header("List of RaycastHit from the two controllers")]
        public List<RaycastHit> RightHits = new List<RaycastHit>();
        public List<RaycastHit> LeftHits = new List<RaycastHit>();

        [Header("The two controllers Ray")]
        public Ray LeftRay;
        public Ray RightRay;

        [Header("The two controllers Position")]
        public Vector3 RightPos;
        public Vector3 LeftPos;
        #endregion PUBLIC_VARIABLES

        //EMPTY
        #region PRIVATE_VARIABLES
        #endregion PRIVATE_VARIABLES

        #region MONOBEHAVIOUR_METHODS
        void Update ()
        {
            if (SetupVR.SDKLoaded.Contains("Simulator"))
                CheckMouseRays();
            else
                CheckVRRays();
        }
        #endregion MONOBEHAVIOUR_METHODS

        //EMPTY
        #region PUBLIC_METHODS
        #endregion PUBLIC_METHODS
            
        #region PRIVATE_METHODS
        /// <summary>
        /// Check the Rays from the two controllers
        /// </summary>
        void CheckVRRays()
        {
            RightPos = RightController.transform.position;
            RightRay = new Ray(RightPos, RightController.transform.TransformDirection(Vector3.forward));
            RightHits = Physics.RaycastAll(RightRay).OrderBy(x => x.distance).ToList();

            LeftPos = LeftController.transform.position;
            LeftRay = new Ray(LeftPos, LeftController.transform.TransformDirection(Vector3.forward));
            LeftHits = Physics.RaycastAll(LeftRay).OrderBy(x => x.distance).ToList();
        }

        /// <summary>
        /// Check the Ray from the Mouse
        /// </summary>
        void CheckMouseRays()
        {
            RightPos = RightController.transform.position;
            RightRay = new Ray(RightPos, RightController.transform.TransformDirection(Vector3.forward));
            RightHits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition)).OrderBy(x => x.distance).ToList();

            LeftPos = LeftController.transform.position;
            LeftRay = new Ray(LeftPos, LeftController.transform.TransformDirection(Vector3.forward));
            LeftHits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition)).OrderBy(x => x.distance).ToList();
        }
        #endregion PRIVATE_METHODS
    }
}
