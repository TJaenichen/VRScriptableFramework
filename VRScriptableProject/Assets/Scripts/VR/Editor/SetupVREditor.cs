using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Framework.VR.Editor
{
    /// <summary>
    /// Need to stay in an Editor Folder.
    /// Customize the SetupVR Prefab in the Inspector.
    /// </summary>
    [CanEditMultipleObjects, CustomEditor(typeof(SetupVR))]
    public class SetupVREditor : UnityEditor.Editor
    {
        #region PRIVATE_VARIABLE
        private string[] _choices = new []{"OVR", "OpenVR", "Simulator"}; 
        private int _choiceIndex;
        private SerializedProperty SDKToLoadProperty;
        #endregion PRIVATE_VARIABLE

        #region MONOBEHAVIOUR_METHODS
        void OnEnable()
        {
            // Setup the SerializedProperties. 
            SDKToLoadProperty = serializedObject.FindProperty("SDKToLoad");

            // Set the choice index to the previously selected index
            _choiceIndex = Array.IndexOf(_choices, SDKToLoadProperty.stringValue);
        }
        #endregion MONOBEHAVIOUR_METHODS

        #region UNITY_EDITOR_METHODS
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            serializedObject.Update();
            _choiceIndex = EditorGUILayout.Popup("XR SDK", _choiceIndex, _choices);
            if (_choiceIndex < 0)
                _choiceIndex = 0;
            SDKToLoadProperty.stringValue = _choices[_choiceIndex];

            // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
            serializedObject.ApplyModifiedProperties();
        }
        #endregion UNITY_EDITOR_METHODS
    }
}