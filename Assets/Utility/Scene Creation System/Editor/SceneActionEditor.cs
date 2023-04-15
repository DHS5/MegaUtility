using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dhs5.Utility.SceneCreation
{
    [CustomPropertyDrawer(typeof(SceneAction))]
    public class SceneActionEditor : PropertyDrawer
    {
        private float propertyHeight;
        private float propertyOffset;
        
        SerializedProperty sceneVariablesSO;
        SerializedObject sceneVariablesObj;
        SceneVariablesSO sceneVarContainer;

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

            sceneVarUniqueIDP = property.FindPropertyRelative("varUniqueID");
            sceneVarIndex = sceneVarContainer.GetIndexByUniqueID(sceneVarUniqueIDP.intValue);
            if (sceneVarIndex == -1) sceneVarIndex = 0;
            sceneVar = sceneVarContainer.sceneVars[sceneVarIndex];

            Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, property.isExpanded ? "" : property.FindPropertyRelative("actionID").stringValue);
            if (property.isExpanded)
            {
                // ActionID property field
                Rect idPosition = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(idPosition, property.FindPropertyRelative("actionID"));
                propertyOffset = EditorGUIUtility.singleLineHeight * 2;
                propertyHeight += EditorGUIUtility.singleLineHeight * 2;

                // SceneVar choice popup
                int sceneVarIndexSave = sceneVarIndex;
                Rect popupPosition = new Rect(position.x, position.y + propertyOffset, position.width * 0.3f, EditorGUIUtility.singleLineHeight);
                sceneVarIndex = EditorGUI.Popup(popupPosition, sceneVarIndex, sceneVarContainer.IDs.ToArray());
                if (sceneVarContainer.GetUniqueIDByIndex(sceneVarIndex) == 0) sceneVarIndex = sceneVarIndexSave;
                sceneVarUniqueIDP.intValue = sceneVarContainer.GetUniqueIDByIndex(sceneVarIndex);

                // Type label
                Rect typePosition = new Rect(position.x + position.width * 0.31f, position.y + propertyOffset, position.width * 0.17f, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(typePosition, sceneVar.type.ToString());

                // Operation creation
                Rect opPosition = new Rect(position.x + position.width * 0.5f, position.y + propertyOffset, position.width * 0.23f, EditorGUIUtility.singleLineHeight);
                Rect valuePosition = new Rect(position.x + position.width * 0.74f, position.y + propertyOffset, position.width * 0.26f, EditorGUIUtility.singleLineHeight);
                SceneVarType type = sceneVar.type;
                switch (type)
                {
                    case SceneVarType.BOOL:
                        EditorGUI.PropertyField(opPosition, property.FindPropertyRelative("boolOP"), new GUIContent(""));
                        if (property.FindPropertyRelative("boolOP").enumValueIndex == 0)
                            EditorGUI.PropertyField(valuePosition, property.FindPropertyRelative("boolValue"), new GUIContent(""));
                        break;
                    case SceneVarType.INT:
                        EditorGUI.PropertyField(opPosition, property.FindPropertyRelative("intOP"), new GUIContent(""));
                        EditorGUI.PropertyField(valuePosition, property.FindPropertyRelative("intValue"), new GUIContent(""));
                        break;
                    case SceneVarType.FLOAT:
                        EditorGUI.PropertyField(opPosition, property.FindPropertyRelative("floatOP"), new GUIContent(""));
                        EditorGUI.PropertyField(valuePosition, property.FindPropertyRelative("floatValue"), new GUIContent(""));
                        break;
                    case SceneVarType.STRING:
                        EditorGUI.PropertyField(opPosition, property.FindPropertyRelative("stringOP"), new GUIContent(""));
                        EditorGUI.PropertyField(valuePosition, property.FindPropertyRelative("stringValue"), new GUIContent(""));
                        break;
                }
                propertyHeight += EditorGUIUtility.singleLineHeight * 1.4f;
                propertyOffset += EditorGUIUtility.singleLineHeight * 1.4f;

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
