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
                propertyOffset = EditorGUIUtility.singleLineHeight * 1.5f;
                propertyHeight += EditorGUIUtility.singleLineHeight * 1.5f;

                // Condition
                hasConditionP = property.FindPropertyRelative("hasCondition");
                Rect togglePosition = new Rect(position.x, position.y + propertyOffset, position.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(togglePosition, hasConditionP);
                propertyHeight += EditorGUIUtility.singleLineHeight * 1.2f;
                propertyOffset += EditorGUIUtility.singleLineHeight * 1.2f;

                // Condition creation
                if (hasConditionP.boolValue)
                {
                    Rect compPosition = new Rect(position.x, position.y + propertyOffset, position.width / 2, EditorGUIUtility.singleLineHeight);
                    Rect valuePosition = new Rect(position.x + position.width * 0.6f, position.y + propertyOffset, position.width * 0.4f, EditorGUIUtility.singleLineHeight);
                    propertyOffset += EditorGUIUtility.singleLineHeight * 1.2f;
                    SceneVarType type = sceneVar.type;
                    switch (type)
                    {
                        case SceneVarType.BOOL:
                            EditorGUI.PropertyField(compPosition, property.FindPropertyRelative("boolComp"), new GUIContent(""));
                            EditorGUI.PropertyField(valuePosition, property.FindPropertyRelative("boolValue"), new GUIContent(""));
                            break;
                        case SceneVarType.INT:
                            EditorGUI.PropertyField(compPosition, property.FindPropertyRelative("intComp"), new GUIContent(""));
                            EditorGUI.PropertyField(valuePosition, property.FindPropertyRelative("intValue"), new GUIContent(""));
                            break;
                        case SceneVarType.FLOAT:
                            EditorGUI.PropertyField(compPosition, property.FindPropertyRelative("floatComp"), new GUIContent(""));
                            EditorGUI.PropertyField(valuePosition, property.FindPropertyRelative("floatValue"), new GUIContent(""));
                            break;
                        case SceneVarType.STRING:
                            EditorGUI.PropertyField(compPosition, property.FindPropertyRelative("stringComp"), new GUIContent(""));
                            EditorGUI.PropertyField(valuePosition, property.FindPropertyRelative("stringValue"), new GUIContent(""));
                            break;
                    }
                    propertyHeight += EditorGUIUtility.singleLineHeight * 1.2f;
                }

                // Event property
                propertyOffset += EditorGUIUtility.singleLineHeight * 0.5f;
                propertyHeight += EditorGUIUtility.singleLineHeight * 0.5f;
                Rect eventPosition = new(position.x, position.y + propertyOffset, position.width, position.height);
                EditorGUI.PropertyField(eventPosition, property.FindPropertyRelative("events"));
                propertyHeight += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("events"), true);
                propertyOffset += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("events"), true);

                // Debug toggle
                Rect debugTogglePosition = new(position.x, position.y + propertyOffset, position.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(debugTogglePosition, property.FindPropertyRelative("debug"));
                propertyHeight += EditorGUIUtility.singleLineHeight;
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
