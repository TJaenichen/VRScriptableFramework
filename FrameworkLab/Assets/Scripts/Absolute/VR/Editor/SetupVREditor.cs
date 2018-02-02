using Absolute.Util;
using System;
using UnityEditor;

namespace Absolute.VR.Editor
{
    [CanEditMultipleObjects, CustomEditor(typeof(GlobalConfig))]
    public class SetupVREditor : UnityEditor.Editor
    {
        private string[] _choices = new []{"Rift", "Vive", "Simulator"};
        private int _choiceIndex;
        private SerializedProperty SDKToLoadProperty;

        void OnEnable()
        {
            // Setup the SerializedProperties.
            SDKToLoadProperty = serializedObject.FindProperty("SDKToLoad");
            // Set the choice index to the previously selected index
            _choiceIndex = Array.IndexOf(_choices, SDKToLoadProperty.stringValue);

        }
      
        // Update is called once per frame
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
    }
}