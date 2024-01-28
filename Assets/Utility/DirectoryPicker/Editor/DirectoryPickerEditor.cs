using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor;
using UnityEngine;

namespace Dhs5.Utility.DirectoryPicker.Editor
{
    [CustomPropertyDrawer(typeof(DirectoryPicker))]
    public class DirectoryPickerEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty pathProp = property.FindPropertyRelative("path");
            if (string.IsNullOrEmpty(pathProp.stringValue))
            {
                pathProp.stringValue = "Assets/";
            }

            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.LabelField(new Rect(position.x, position.y, position.width * 0.38f, position.height), new GUIContent(label));
            GUI.Box(new Rect(position.x + position.width * 0.40f, position.y, position.width * 0.54f, position.height), "", GUI.skin.window);
            EditorGUI.SelectableLabel(new Rect(position.x + position.width * 0.41f, position.y, position.width * 0.52f, position.height)
                , pathProp.stringValue);
            if (GUI.Button(new Rect(position.x + position.width * 0.95f, position.y, position.width * 0.05f, position.height)
                , EditorGUIUtility.IconContent("FolderOpened On Icon")))
            {
                string absPath = EditorUtility.OpenFolderPanel(label.text, pathProp.stringValue, "");
                if (!string.IsNullOrEmpty(absPath))
                {
                    pathProp.stringValue = absPath.Substring(absPath.IndexOf("Assets/"));
                    property.serializedObject.ApplyModifiedProperties();
                }
                GUIUtility.ExitGUI();
            }

            EditorGUI.EndProperty();
        }
    }
}
