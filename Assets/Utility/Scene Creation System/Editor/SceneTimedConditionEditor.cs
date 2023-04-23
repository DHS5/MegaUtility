using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dhs5.Utility.SceneCreation
{
    [CustomPropertyDrawer(typeof(SceneTimedCondition))]
    public class SceneTimedConditionEditor : PropertyDrawer
    {
        private float propertyOffset;
        
        private SerializedProperty conditionTypeProperty;
        private SerializedProperty timeToWaitProperty;
        private SerializedProperty sceneConditionsProperty;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            propertyOffset = 0;
            
            conditionTypeProperty = property.FindPropertyRelative("conditionType");
            timeToWaitProperty = property.FindPropertyRelative("timeToWait");
            sceneConditionsProperty = property.FindPropertyRelative("sceneConditions");

            EditorGUI.BeginProperty(position, label, property);
            
            Rect foldoutPosition = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(foldoutPosition, property.isExpanded, label);
            propertyOffset += EditorGUIUtility.singleLineHeight;
            if (property.isExpanded)
            {
                // Condition type choice
                Rect typePosition = new Rect(position.x, position.y + propertyOffset, position.width,
                    EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(typePosition, conditionTypeProperty);
                propertyOffset += EditorGUIUtility.singleLineHeight;
                
                // Param
                Rect paramPosition = new Rect(position.x, position.y + propertyOffset, position.width,
                    EditorGUIUtility.singleLineHeight);
                switch ((SceneTimedCondition.TimedConditionType)conditionTypeProperty.enumValueIndex)
                {
                    case SceneTimedCondition.TimedConditionType.WAIT_FOR_TIME:
                        EditorGUI.PropertyField(paramPosition, timeToWaitProperty);
                        propertyOffset += EditorGUIUtility.singleLineHeight * 1.5f;
                        break;
                    case SceneTimedCondition.TimedConditionType.WAIT_UNTIL_SCENE_CONDITION:
                        EditorGUI.PropertyField(paramPosition, sceneConditionsProperty);
                        propertyOffset += EditorGUI.GetPropertyHeight(sceneConditionsProperty) + EditorGUIUtility.singleLineHeight * 0.15f;
                        break;
                    case SceneTimedCondition.TimedConditionType.WAIT_WHILE_SCENE_CONDITION:
                        EditorGUI.PropertyField(paramPosition, sceneConditionsProperty);
                        propertyOffset += EditorGUI.GetPropertyHeight(sceneConditionsProperty) + EditorGUIUtility.singleLineHeight * 0.15f;
                        break;
                }
                
                Rect line1Rect = new Rect(position.x, position.y, position.width, 1);
                Rect line2Rect = new Rect(position.x, position.y + propertyOffset, position.width, 1);
                EditorGUI.DrawRect(line1Rect, Color.white);
                EditorGUI.DrawRect(line2Rect, Color.white);
            }
            
            // End
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            conditionTypeProperty = property.FindPropertyRelative("conditionType");
            sceneConditionsProperty = property.FindPropertyRelative("sceneConditions");
            return property.isExpanded ? 
                conditionTypeProperty.enumValueIndex == 0 ? EditorGUIUtility.singleLineHeight * 3.8f :
                    EditorGUI.GetPropertyHeight(sceneConditionsProperty) + EditorGUIUtility.singleLineHeight * 2.4f
                    : EditorGUIUtility.singleLineHeight * 1.4f;
        }
    }
}
