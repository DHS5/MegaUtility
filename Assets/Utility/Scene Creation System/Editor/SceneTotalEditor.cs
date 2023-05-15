using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dhs5.Utility.SceneCreation
{
    [CustomPropertyDrawer(typeof(SceneTotal))]
    public class SceneTotalEditor : PropertyDrawer
    {
        SerializedProperty sceneVariablesSO;
        SerializedObject sceneVariablesObj;
        SceneVariablesSO sceneVarContainer;

        GUIContent empty = new GUIContent("");

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SceneVarType type = (SceneVarType)property.FindPropertyRelative("type").enumValueIndex;

            Rect varPosition = new Rect(position.x, position.y, position.width * (type != SceneVarType.STRING ? 0.75f : 1), EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(varPosition, property.FindPropertyRelative("varTween"), empty);

            // Math Operator property
            if (type != SceneVarType.STRING)
            {
                Rect opPosition = new Rect(position.x + position.width * 0.8f, position.y + EditorGUIUtility.singleLineHeight * 0.5f, position.width * 0.2f, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(opPosition, property.FindPropertyRelative("op"), empty);
            }

            // End
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 1.5f;
        }
    }
}
