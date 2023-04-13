using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SceneCreation
{
    [CustomPropertyDrawer(typeof(SceneListener))]
    public class SceneListenerEditor : PropertyDrawer
    {
        private float propertyHeight;
        private float propertyOffset;
        
        SerializedProperty sceneVariablesSO;
        SerializedObject sceneVariablesObj;
        SerializedProperty sceneVars;

        private SerializedProperty hasConditionP;
        private SerializedProperty sceneVarUniqueIDP;

        int sceneVarIndex = 0;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            propertyHeight = base.GetPropertyHeight(property, label);
            sceneVarIndex = 0;
            
            EditorGUI.BeginProperty(position, label, property);

            sceneVariablesSO = property.FindPropertyRelative("sceneVariablesSO");
            if (sceneVariablesSO == null)
            {
                EditorGUI.LabelField(position, "SceneVariablesSO is missing !");
                EditorGUI.EndProperty();
                return;
            }
            
            // Get the SceneVars from the SceneVariablesSO
            sceneVariablesObj = new SerializedObject(sceneVariablesSO.objectReferenceValue);
            sceneVars = sceneVariablesObj.FindProperty("sceneVars");
            sceneVarUniqueIDP = property.FindPropertyRelative("varUniqueID");

            List<string> varIDs = new();
            List<int> varUniqueIDs = new();
            List<int> varTypes = new();
            int j = 0;
            for (int i = 0; i < sceneVars.arraySize; i++)
            {
                if (sceneVars.GetArrayElementAtIndex(i).FindPropertyRelative("uniqueID").intValue != 0)
                {
                    varUniqueIDs.Add(sceneVars.GetArrayElementAtIndex(i).FindPropertyRelative("uniqueID").intValue);
                    varIDs.Add(sceneVars.GetArrayElementAtIndex(i).FindPropertyRelative("ID").stringValue);
                    varTypes.Add(sceneVars.GetArrayElementAtIndex(i).FindPropertyRelative("type").enumValueIndex);
                    if (sceneVars.GetArrayElementAtIndex(i).FindPropertyRelative("uniqueID").intValue ==
                        sceneVarUniqueIDP.intValue)
                    {
                        sceneVarIndex = j;
                    }
                    j++;
                }
            }
            
            // SceneVar choice popup
            Rect popupPosition = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            sceneVarIndex = EditorGUI.Popup(popupPosition, sceneVarIndex, varIDs.ToArray());
            sceneVarUniqueIDP.intValue = varUniqueIDs[sceneVarIndex];
            propertyOffset = EditorGUIUtility.singleLineHeight;
            
            // Type label
            Rect typePosition = new Rect(position.x, position.y + propertyOffset, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(typePosition, "Type", ((SceneVarType) varTypes[sceneVarIndex]).ToString());
            propertyHeight += EditorGUIUtility.singleLineHeight * 2;
            propertyOffset += EditorGUIUtility.singleLineHeight * 2;
            
            // Condition
            hasConditionP = property.FindPropertyRelative("hasCondition");
            Rect togglePosition = new Rect(position.x, position.y + propertyOffset, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(togglePosition, hasConditionP);
            propertyHeight += EditorGUIUtility.singleLineHeight;
            propertyOffset += EditorGUIUtility.singleLineHeight;
            
            // Condition creation
            if (hasConditionP.boolValue)
            {
                Rect compPosition = new Rect(position.x, position.y + propertyOffset, position.width, EditorGUIUtility.singleLineHeight);
                propertyOffset += EditorGUIUtility.singleLineHeight;
                Rect valuePosition = new Rect(position.x, position.y + propertyOffset, position.width, EditorGUIUtility.singleLineHeight);
                propertyOffset += EditorGUIUtility.singleLineHeight;
                SceneVarType type = (SceneVarType) varTypes[sceneVarIndex];
                switch (type)
                {
                    case SceneVarType.BOOL:
                        EditorGUI.PropertyField(compPosition, property.FindPropertyRelative("boolComp"));
                        EditorGUI.PropertyField(valuePosition, property.FindPropertyRelative("boolValue"));
                        break;
                    case SceneVarType.INT:
                        EditorGUI.PropertyField(compPosition, property.FindPropertyRelative("intComp"));
                        EditorGUI.PropertyField(valuePosition, property.FindPropertyRelative("intValue"));
                        break;
                    case SceneVarType.FLOAT:
                        EditorGUI.PropertyField(compPosition, property.FindPropertyRelative("floatComp"));
                        EditorGUI.PropertyField(valuePosition, property.FindPropertyRelative("floatValue"));
                        break;
                    case SceneVarType.STRING:
                        EditorGUI.PropertyField(compPosition, property.FindPropertyRelative("stringComp"));
                        EditorGUI.PropertyField(valuePosition, property.FindPropertyRelative("stringValue"));
                        break;
                }
                propertyHeight += EditorGUIUtility.singleLineHeight * 2;
            }

            // Event property
            propertyOffset += EditorGUIUtility.singleLineHeight;
            propertyHeight += EditorGUIUtility.singleLineHeight;
            Rect eventPosition = new(position.x, position.y + propertyOffset, position.width, position.height);
            EditorGUI.PropertyField(eventPosition, property.FindPropertyRelative("events"));
            propertyHeight += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("events"), true);

            // End
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return propertyHeight;
        }
    }
}
