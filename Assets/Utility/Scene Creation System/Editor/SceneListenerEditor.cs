using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dhs5.Utility.SceneCreation
{
    [CustomPropertyDrawer(typeof(SceneListener))]
    public class SceneListenerEditor : PropertyDrawer
    {
        SerializedProperty sceneVariablesSO;
        SerializedObject sceneVariablesObj;
        SerializedProperty sceneVars;

        int sceneVarIndex = 0;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            sceneVariablesSO = property.FindPropertyRelative("sceneVariablesSO");
            sceneVariablesObj = new SerializedObject(sceneVariablesSO.objectReferenceValue);
            sceneVars = sceneVariablesObj.FindProperty("sceneVars");

            List<string> varIDs = new();
            for (int i = 0; i < sceneVars.arraySize; i++)
            {
                varIDs.Add(sceneVars.GetArrayElementAtIndex(i).FindPropertyRelative("ID").stringValue);
            }

            Rect popupPosition = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            sceneVarIndex = EditorGUI.Popup(popupPosition, sceneVarIndex, varIDs.ToArray());

            property.FindPropertyRelative("varIndex").intValue = sceneVarIndex;

            Rect eventPosition = new(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, position.height);
            EditorGUI.PropertyField(eventPosition, property.FindPropertyRelative("events"));

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) + EditorGUI.GetPropertyHeight(property.FindPropertyRelative("events"), true);
        }
    }
}
