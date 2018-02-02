#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.XR;
using Absolute.Util;

namespace Absolute.VR
{
#if UNITY_EDITOR
    [CanEditMultipleObjects]
#endif
    public class SetupVR : MonoBehaviour
    {

        #region PUBLIC_VARIABLES
        public static GameObject ActiveSDK;
        public static string SDKLoaded;

        public GameObject ViveSDK;
        public GameObject OculusSDK;
        public GameObject SimulatorSDK;
        public PostProcessingProfile PostProcessingProfile;
        //[HideInInspector]
        //public string SDKToLoad;
        #endregion


        #region PRIVATE_VARIABLES
        private static Transform RightController;
        private static Transform LeftController;

        private bool _loaded;  //True when SDK Instantiate, false when everything is setup
        private bool _setPostProc;
        private bool _playerPositionned;
        private GameObject _sdk;
        #endregion


        #region MONOBEHAVIOUR_METHODS
        private void Awake()
        {
            foreach (var commandLineArg in System.Environment.GetCommandLineArgs())
            {
                if (commandLineArg.ToUpper().Contains("RIFT"))
                {
                    GlobalConfig.Instance.SDKToLoad = "Rift";
                    break;
                }
                if (commandLineArg.ToUpper().Contains("VIVE"))
                {
                    GlobalConfig.Instance.SDKToLoad = "Vive";
                    break;
                }
                if (commandLineArg.ToUpper().Contains("SIMULATOR"))
                {
                    GlobalConfig.Instance.SDKToLoad = "Simulator";
                    break;
                }
            }

            // Default to Vive
            if (!string.IsNullOrEmpty(GlobalConfig.Instance.SDKToLoad))
            {
                if (GlobalConfig.Instance.SDKToLoad.Contains("Rift"))
                {
                    XRSettings.enabled = true;
                    _sdk = Instantiate(OculusSDK);
                    _sdk.transform.name = OculusSDK.name;
                    //sdk.transform.position = new Vector3(sdk.transform.position.x, 200, sdk.transform.position.z);
                    //Active SDK is set to the cameraRig, as it's the only object that will be moved
                    ActiveSDK = _sdk.transform.GetChild(0).gameObject;
                    SDKLoaded = "Rift";
                }
                else if (GlobalConfig.Instance.SDKToLoad.Contains("Vive"))
                {
                    XRSettings.enabled = true;
                    GameObject sdk = Instantiate(ViveSDK);
                    sdk.transform.name = ViveSDK.name;
                    //Active SDK is set to the cameraRig, as it's the only object that will be moved
                    ActiveSDK = sdk.transform.GetChild(0).gameObject;
                    SDKLoaded = "Vive";
                } else if (GlobalConfig.Instance.SDKToLoad.Contains("Simulator"))
                {
                    XRSettings.enabled = false;
                    GameObject sdk = Instantiate(SimulatorSDK);
                    sdk.transform.name = SimulatorSDK.name;
                    //Active SDK is set to the cameraRig, as it's the only object that will be moved
                    ActiveSDK = sdk.transform.GetChild(0).gameObject;
                    SDKLoaded = "Simulator";
                }
            }
            else
            {
                XRSettings.enabled = true;
                GameObject sdk = Instantiate(ViveSDK);
                sdk.transform.name = ViveSDK.name;
                //Active SDK is set to the cameraRig, as it's the only object that will be moved
                ActiveSDK = sdk.transform.GetChild(0).gameObject;
                SDKLoaded = "Vive";
            }
        }

        void Update()
        {
            if (!_loaded && GameObject.FindGameObjectWithTag("CameraRig") != null)
            {
                _loaded = true;
                return;
            }

            if (ActiveSDK == null)
            {
                ActiveSDK = GameObject.FindGameObjectWithTag("CameraRig");
                return;
            }

            //To setup the controllers reference
            if (_loaded && (RightController == null || LeftController == null))
            {
                try
                {
                    LeftController = GameObject.Find("LeftController").transform;   //Always attached to the controller, contains scripts 
                    RightController = GameObject.Find("RightController").transform;   //Always attached to the controller, contains scripts 
                    _loaded = false;
                } catch
                {
                    Debug.LogError("Can't setup Left and Right Controllers");
                }
            }

            if (_loaded && !_playerPositionned)
            {
                SetupPlayerPos();
            }

            if (_loaded && !_setPostProc)
            {
                var cam = Camera.main;
                if (cam == null)
                {
                    return;
                }
                PostProcessingBehaviour ppb;
                try
                {
                    ppb = cam.gameObject.GetComponent<PostProcessingBehaviour>();
                } catch
                {
                    ppb = cam.gameObject.AddComponent<PostProcessingBehaviour>();
                }
                ppb.profile = PostProcessingProfile;// Resources.Load<PostProcessingProfile>("PostProcessingBehaviours/Absolute");
                _setPostProc = true;
            }
        }
        #endregion

        #region PRIVATE_METHODS
        /// <summary>
        /// Setup player position at runtime. Datas come from ini file, read by GlobalConfig class
        /// </summary>
        void SetupPlayerPos()
        {
            var x = GlobalConfig.Instance.StartingLat;
            var z = GlobalConfig.Instance.StartingLong;
            var pos = GpsPosition.GPSToUnity(x, z);
            ActiveSDK.transform.position = new Vector3(pos.x, 20, pos.z);
            if (ActiveSDK.transform.position.x != 0 && ActiveSDK.transform.position.z != 0)
            {
                _playerPositionned = true;
            }
        }
        #endregion

        #region GETTERS_SETTERS
        /// <summary>
        /// Get the position of the active right controller from the VRTKSDK_Setup
        /// </summary>
        public static Vector3 RightControllerPosition
        {
            get
            {
                if (RightController == null)
                {
                    return Vector3.zero;
                }
                return RightController.transform.TransformPoint(
                    RightController.transform.position);
            }
        }

        /// <summary>
        /// Get the Transform of the active right controller from the VRTKSDK_Setup
        /// </summary>
        public static Transform RightControllerTransform
        {
            get
            {
                if (RightController == null)
                {
                    return null;
                }
                return RightController.transform;
            }
        }

        /// <summary>
        /// Get the position of the active left controller from the VRTKSDK_Setup
        /// </summary>
        public static Vector3 LeftControllerPosition
        {
            get
            {
                if (LeftController == null)
                {
                    return Vector3.zero;
                }
                return LeftController.transform.TransformPoint(
                    LeftController.transform.position);
            }
        }

        /// <summary>
        /// Get the Transform of the active Left controller from the VRTKSDK_Setup
        /// </summary>
        public static Transform LeftControllerTransform
        {
            get
            {
                if (LeftController == null)
                {
                    return null;
                }
                return LeftController.transform;
            }
        }
        #endregion
    }
}