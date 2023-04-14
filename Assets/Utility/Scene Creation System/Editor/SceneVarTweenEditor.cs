using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dhs5.Utility.SceneCreation
{
    [CustomPropertyDrawer(typeof(SceneVarTween))]
    public class SceneVarTweenEditor : PropertyDrawer
    {
        SerializedProperty sceneVariablesSO;
        SerializedObject sceneVariablesObj;
        SceneVariablesSO sceneVarContainer;

        private SerializedProperty sceneVarUniqueIDP;

        int sceneVarIndex = 0;
        int sceneVarIndexSave = 0;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            sceneVarIndex = 0;

            EditorGUI.BeginProperty(position, label, property);

            sceneVariablesSO = property.FindPropertyRelative("sceneVariablesSO");
            if (sceneVariablesSO.objectReferenceValue == null)
            {
                EditorGUI.LabelField(position, "SceneVariablesSO is not assigned !");
                EditorGUI.EndProperty();
                return;
            }
            // Get the SceneVariablesSO
            sceneVariablesObj = new SerializedObject(sceneVariablesSO.objectReferenceValue);
            sceneVarContainer = sceneVariablesObj.targetObject as SceneVariablesSO;
            if (sceneVarContainer == null)
            {
                EditorGUI.LabelField(position, "SceneVariablesSO is null !");
                EditorGUI.EndProperty();
                return;
            }

            sceneVarUniqueIDP = property.FindPropertyRelative("sceneVarUniqueID");
            sceneVarIndexSave = sceneVarContainer.GetIndexByUniqueID(sceneVarUniqueIDP.intValue);

            // Label
            Rect labelPosition = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 0.25f, position.width * 0.3f, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(labelPosition, property.displayName);

            // SceneVar choice popup
            Rect popupPosition = new Rect(position.x + position.width * 0.32f, position.y + EditorGUIUtility.singleLineHeight * 0.25f, position.width * 0.52f, EditorGUIUtility.singleLineHeight);
            sceneVarIndex = EditorGUI.Popup(popupPosition, sceneVarIndexSave, sceneVarContainer.IDs.ToArray());
            if (sceneVarContainer.GetUniqueIDByIndex(sceneVarIndex) == 0) sceneVarIndex = sceneVarIndexSave;
            sceneVarUniqueIDP.intValue = sceneVarContainer.GetUniqueIDByIndex(sceneVarIndex);

            // Label
            Rect typePosition = new Rect(position.x + position.width * 0.85f, position.y + EditorGUIUtility.singleLineHeight * 0.25f, position.width * 0.18f, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(typePosition, sceneVarContainer[sceneVarUniqueIDP.intValue].type.ToString());

            // End
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 1.5f;
        }
    }
}
