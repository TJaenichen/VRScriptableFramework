using Framework.Variables;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Framework.VR.Utils
{
    /// <summary>
    /// Check the Raycast of the two controllers and the Gaze and reference them in three list of RaycastHit
    /// </summary>
    public class PointerRayCast : MonoBehaviour
    {
        #region PUBLIC_VARIABLES
        [Header("The two controllers GameObject.")]
        public GameObject RightController;
        public GameObject LeftController;

        [Header("The Gaze Parameters, if you use it.")]
        [Tooltip("Set this to true if you want to use the Gaze feature.")]
        public BoolVariable UseGaze;
        
        [Header("OPTIONAL : Layer to exclude from the raycast.")]
        [Tooltip("Everything link to this layer will be ignored by the Raycast and the Raycast Hit")]
        public LayerMask ExclusionLayer;
        #endregion PUBLIC_VARIABLES
        
        #region PRIVATE_VARIABLES
        [Tooltip("List of RaycastHit from the two controllers and the Gaze, if used.")]
        private List<RaycastHit> rightHits = new List<RaycastHit>();
        private List<RaycastHit> leftHits = new List<RaycastHit>();
        private List<RaycastHit> gazeHits = new List<RaycastHit>();

        [Tooltip("The Ray of the two controllers and the Gaze, if used.")]
        private Ray rightRay;
        private Ray leftRay;
        private Ray gazeRay;

        [Header("The two controllers and the gaze, if used, positions.")]
        private Vector3 rightPos;
        private Vector3 leftPos;
        private Vector3 gazePos;
        #endregion PRIVATE_VARIABLES

        #region MONOBEHAVIOUR_METHODS
        void Update ()
        {
            if (SetupVR.DeviceLoaded == Device.SIMULATOR)
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
            rightPos = RightController.transform.position;
            rightRay = new Ray(rightPos, RightController.transform.TransformDirection(Vector3.forward));
            rightHits = Physics.RaycastAll(rightRay).OrderBy(x => x.distance).ToList();

            leftPos = LeftController.transform.position;
            leftRay = new Ray(leftPos, LeftController.transform.TransformDirection(Vector3.forward));
            leftHits = Physics.RaycastAll(leftRay).OrderBy(x => x.distance).ToList();

            if (UseGaze)
            {
                gazePos = transform.position;
                gazeRay = new Ray(gazePos, transform.TransformDirection(Vector3.forward));
                gazeHits = Physics.RaycastAll(GazeRay).OrderBy(x => x.distance).ToList();
            }
        }

        /// <summary>
        /// Check the Ray from the Mouse
        /// </summary>
        void CheckMouseRays()
        {
            rightPos = RightController.transform.position;
            rightRay = new Ray(rightPos, RightController.transform.TransformDirection(Vector3.forward));
            rightHits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition)).OrderBy(x => x.distance).ToList();

            leftPos = LeftController.transform.position;
            leftRay = new Ray(leftPos, LeftController.transform.TransformDirection(Vector3.forward));
            leftHits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition)).OrderBy(x => x.distance).ToList();

            if (UseGaze)
            {
                gazePos = transform.position;
                gazeRay = new Ray(gazePos, transform.TransformDirection(Vector3.forward));
                gazeHits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition)).OrderBy(x => x.distance).ToList();
            }
        }
        #endregion PRIVATE_METHODS

        #region GETTERS_SETTERS

        public List<RaycastHit> RightHits
        {
            get { return rightHits; }
        }

        public List<RaycastHit> LeftHits
        {
            get { return leftHits; }
        }

        public List<RaycastHit> GazeHits
        {
            get { return gazeHits; }
        }

        public Ray RightRay
        {
            get { return rightRay; }
        }

        public Ray LeftRay
        {
            get { return leftRay; }
        }

        public Ray GazeRay
        {
            get { return gazeRay; }
        }

        public Vector3 RightPos
        {
            get { return rightPos; }
        }

        public Vector3 LeftPos
        {
            get { return leftPos; }
        }

        public Vector3 GazePos
        {
            get { return gazePos; }
        }
        #endregion GETTERS_SETTERS
    }
}
