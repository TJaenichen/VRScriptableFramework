﻿#if UNITY_EDITOR
using Framework.Events;
using Framework.Util;
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

namespace Framework.VR
{
    /// <summary>
    /// Script placed on the SetupVR Prefab.
    /// Load the selected SDK at runtime, and stock all the references to the important VR components.
    /// </summary>
#if UNITY_EDITOR
    [CanEditMultipleObjects]
#endif
    public class SetupVR : MonoBehaviour
    {
        #region PUBLIC_VARIABLES
        [Tooltip("A reference to the CameraRig object")]
        public static GameObject ActiveSDK;

        [Tooltip("The name of the SDK that has been loaded")]
        public static string SDKLoaded;

        [Header("OPTIONAL : The World Position to set the CameraRig at runtime.")]
        public Vector3 StartingPoint;

        [Header("OPTIONAL : The Scene where to load the VR SDK")]
        [Tooltip("If Empty, the current scene will be use")]
        public string SceneToUse;

        [Header("The 3 prefabs to load for the Vive, Oculus and Simulator.")]
        public GameObject ViveSDK;
        public GameObject OculusSDK;
        public GameObject SimulatorSDK;
        #endregion

        #region PRIVATE_VARIABLES
        [Tooltip("The name of the SDK that needs to be loaded")]
        [SerializeField] [HideInInspector]
        private string SDKToLoad;

        private static Transform RightControllerScripts;
        private static Transform LeftControllerScripts;

        private bool _loaded;
        private bool _controllerSetup;
        private bool _playerPositionned;
        private bool setupEnded;
        private GameObject _sdk;
        #endregion

        #region MONOBEHAVIOUR_METHODS
        private void Awake()
        {
            DontDestroyOnLoad(this);
            CheckCommandLine();
        }

        void Update()
        {
            if (!setupEnded)
            {
                if (!string.IsNullOrEmpty(SceneToUse))
                {
                    var sceneName = SceneManager.GetActiveScene().name;
                    if (!sceneName.Contains(SceneToUse))
                        return;
                }

                if (!_loaded)
                    LoadCorrespondingSDK();

                if (!_loaded && ActiveSDK != null)
                {
                    _loaded = true;
                }
                else if (ActiveSDK == null)
                {
                    ActiveSDK = GameObject.FindGameObjectWithTag("CameraRig").gameObject;
                    return;
                }

                if (!CheckControllersReferences())
                    return;

                if (_loaded && !SetupPlayerPos())
                    return;

                setupEnded = true;
            }
        }
        #endregion

        #region PRIVATE_METHODS
        /// <summary>
        /// Check which SDK to Load
        /// </summary>
        void CheckCommandLine()
        {
            foreach (var commandLineArg in System.Environment.GetCommandLineArgs())
            {
                if (commandLineArg.ToUpper().Contains("RIFT"))
                {
                    SDKToLoad = "Rift";
                    break;
                }
                else if (commandLineArg.ToUpper().Contains("VIVE"))
                {
                    SDKToLoad = "Vive";
                    break;
                }
                else if (commandLineArg.ToUpper().Contains("SIMULATOR"))
                {
                    SDKToLoad = "Simulator";
                    break;
                }
            }
        }

        /// <summary>
        /// Will Instantiate and reference the SDK prefab to load thanks to the string field.
        /// </summary>
        void LoadCorrespondingSDK()
        {
            // Default to Simulator
            if (!string.IsNullOrEmpty(SDKToLoad))
            {
                if (SDKToLoad.Contains("Rift"))
                {
                    XRSettings.enabled = true;
                    _sdk = Instantiate(OculusSDK);
                    _sdk.transform.name = OculusSDK.name;
                    //Active SDK is set to the cameraRig, as it's the only object that will be moved
                    ActiveSDK = _sdk.transform.GetChild(0).gameObject;
                    SDKLoaded = "Rift";
                }
                else if (SDKToLoad.Contains("Vive"))
                {
                    XRSettings.enabled = true;
                    GameObject sdk = Instantiate(ViveSDK);
                    sdk.transform.name = ViveSDK.name;
                    //Active SDK is set to the cameraRig, as it's the only object that will be moved
                    ActiveSDK = sdk.transform.GetChild(0).gameObject;
                    SDKLoaded = "Vive";
                }
                else if (SDKToLoad.Contains("Simulator"))
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
                XRSettings.enabled = false;
                GameObject sdk = Instantiate(SimulatorSDK);
                sdk.transform.name = SimulatorSDK.name;
                //Active SDK is set to the cameraRig, as it's the only object that will be moved
                ActiveSDK = sdk.transform.GetChild(0).gameObject;
                SDKLoaded = "Simulator";
            }
        }

        /// <summary>
        /// To setup the controllers reference
        /// </summary>
        bool CheckControllersReferences()
        {
            if (_loaded && (RightControllerScripts == null || LeftControllerScripts == null))
            {
                try
                {
                    LeftControllerScripts = ActiveSDK.transform.FindDeepChild("LeftControllerScripts");
                    RightControllerScripts = ActiveSDK.transform.FindDeepChild("RightControllerScripts");

                    if (LeftControllerScripts != null && RightControllerScripts != null)
                        return true;
                    else
                        return false;
                }
                catch
                {
                    Debug.LogError("Can't setup Left and Right Controllers : " + ActiveSDK);
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Setup player position at runtime.
        /// </summary>
        bool SetupPlayerPos()
        {
            try
            {
                ActiveSDK.transform.position = StartingPoint;
                return true;
            }
            catch
            {
                Debug.LogError("Couldn't set CameraRig StartingPoint : " + StartingPoint);
                return false;
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
                if (RightControllerScripts == null)
                {
                    return Vector3.zero;
                }
                return RightControllerScripts.transform.TransformPoint(
                    RightControllerScripts.transform.position);
            }
        }

        /// <summary>
        /// Get the Transform of the active right controller from the VRTKSDK_Setup
        /// </summary>
        public static Transform RightControllerTransform
        {
            get
            {
                if (RightControllerScripts == null)
                {
                    return null;
                }
                return RightControllerScripts.transform;
            }
        }

        /// <summary>
        /// Get the position of the active left controller from the VRTKSDK_Setup
        /// </summary>
        public static Vector3 LeftControllerPosition
        {
            get
            {
                if (LeftControllerScripts == null)
                {
                    return Vector3.zero;
                }
                return LeftControllerScripts.transform.TransformPoint(
                    LeftControllerScripts.transform.position);
            }
        }

        /// <summary>
        /// Get the Transform of the active Left controller from the VRTKSDK_Setup
        /// </summary>
        public static Transform LeftControllerTransform
        {
            get
            {
                if (LeftControllerScripts == null)
                {
                    return null;
                }
                return LeftControllerScripts.transform;
            }
        }
        #endregion
    }
}