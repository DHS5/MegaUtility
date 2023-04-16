using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dhs5.Utility.SceneCreation
{
    [CustomPropertyDrawer(typeof(SceneListener))]
    public class SceneListenerEditor : PropertyDrawer
    {
        private float propertyHeight;
        private float propertyOffset;
        
        SerializedProperty sceneVariablesSO;
        SerializedObject sceneVariablesObj;
        SceneVariablesSO sceneVarContainer;

        private SerializedProperty hasConditionP;
        private SerializedProperty conditionP;
        private SerializedProperty sceneVarUniqueIDP;

        int sceneVarIndex = 0;
        SceneVar sceneVar;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            propertyHeight = 0;
            sceneVarIndex = 0;
            
            EditorGUI.BeginProperty(position, label, property);

            sceneVariablesSO = property.FindPropertyRelative("sceneVariablesSO");
            if (sceneVariablesSO.objectReferenceValue == null)
            {
                EditorGUI.LabelField(position, "SceneVariablesSO is missing !");
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

            sceneVarUniqueIDP = property.FindPropertyRelative("varUniqueID");
            sceneVarIndex = sceneVarContainer.GetIndexByUniqueID(sceneVarUniqueIDP.intValue);
            if (sceneVarIndex == -1) sceneVarIndex = 0;
            sceneVar = sceneVarContainer.sceneVars[sceneVarIndex];

            Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, sceneVar.ID + " : " + sceneVar.type);
            if (property.isExpanded)
            {
                // SceneVar choice popup
                int sceneVarIndexSave = sceneVarIndex;
                Rect popupPosition = new Rect(position.x, position.y, position.width * 0.6f, EditorGUIUtility.singleLineHeight);
                sceneVarIndex = EditorGUI.Popup(popupPosition, sceneVarIndex, sceneVarContainer.IDs.ToArray());
                if (sceneVarContainer.GetUniqueIDByIndex(sceneVarIndex) == 0) sceneVarIndex = sceneVarIndexSave;
                sceneVarUniqueIDP.intValue = sceneVarContainer.GetUniqueIDByIndex(sceneVarIndex);

                // Type label
                Rect typePosition = new Rect(position.x + position.width * 0.65f, position.y, position.width * 0.3f, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(typePosition, sceneVar.type.ToString());
                propertyOffset = EditorGUIUtility.singleLineHeight * 1.2f;
                propertyHeight += EditorGUIUtility.singleLineHeight * 1.2f;

                // Condition
                hasConditionP = property.FindPropertyRelative("hasCondition");
                Rect togglePosition = new Rect(position.x, position.y + propertyOffset, position.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(togglePosition, hasConditionP, new GUIContent("Conditionned"));
                propertyHeight += EditorGUIUtility.singleLineHeight;
                propertyOffset += EditorGUIUtility.singleLineHeight;

                // Condition creation
                if (hasConditionP.boolValue)
                {
                    conditionP = property.FindPropertyRelative("conditions");
                    Rect compPosition = new Rect(position.x, position.y + propertyOffset, position.width, EditorGUIUtility.singleLineHeight);
                    EditorGUI.PropertyField(compPosition, conditionP);
                    propertyOffset += EditorGUI.GetPropertyHeight(conditionP);
                    propertyHeight += EditorGUI.GetPropertyHeight(conditionP);
                }

                // Event property
                propertyOffset += EditorGUIUtility.singleLineHeight * 0.25f;
                propertyHeight += EditorGUIUtility.singleLineHeight * 0.25f;
                Rect eventPosition = new(position.x, position.y + propertyOffset, position.width, position.height);
                EditorGUI.PropertyField(eventPosition, property.FindPropertyRelative("events"));
                propertyHeight += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("events"), true) - EditorGUIUtility.singleLineHeight * 0.5f;
                propertyOffset += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("events"), true) - EditorGUIUtility.singleLineHeight * 0.75f;

                // Debug toggle
                Rect debugTogglePosition = new(position.x, position.y + propertyOffset, position.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(debugTogglePosition, property.FindPropertyRelative("debug"));
                propertyHeight += EditorGUIUtility.singleLineHeight * 0.85f;
            }
            // End
            EditorGUI.EndProperty();

            property.FindPropertyRelative("propertyHeight").floatValue = propertyHeight;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return property.isExpanded ? property.FindPropertyRelative("propertyHeight").floatValue : EditorGUIUtility.singleLineHeight;
        }
    }
}
