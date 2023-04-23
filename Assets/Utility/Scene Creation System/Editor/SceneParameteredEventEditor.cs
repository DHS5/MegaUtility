using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dhs5.Utility.SceneCreation
{
    [CustomPropertyDrawer(typeof(SceneParameteredEvent))]
    public class SceneParameteredEventEditor : PropertyDrawer
    {
        private float propertyHeight;
        private float propertyOffset;

        SerializedProperty sceneVariablesSO;
        SerializedObject sceneVariablesObj;
        SceneVariablesSO sceneVarContainer;

        private SerializedProperty sceneVarUniqueIDP;
        private SerializedProperty eventProperty;
        private SerializedProperty paramTypeProperty;
        private SerializedProperty conditionsProperty;

        int sceneVarIndex = 0;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            propertyHeight = 0;
            propertyOffset = 0;
            sceneVarIndex = 0;

            eventProperty = property.FindPropertyRelative("events");
            paramTypeProperty = property.FindPropertyRelative("parameterType");
            conditionsProperty = property.FindPropertyRelative("conditionsParam");

            EditorGUI.BeginProperty(position, label, property);

            Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, "");
            if (property.isExpanded)
            {
                // UnityEvent
                EditorGUI.PropertyField(position, eventProperty);
                propertyHeight += EditorGUI.GetPropertyHeight(eventProperty) + EditorGUIUtility.singleLineHeight * 0.5f;
                propertyOffset += EditorGUI.GetPropertyHeight(eventProperty) + EditorGUIUtility.singleLineHeight * 0.5f;

                // Parameter type
                Rect paramTypePosition = new Rect(position.x, position.y + propertyOffset, position.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(paramTypePosition, paramTypeProperty, new GUIContent("Parameter"));
                propertyOffset += EditorGUIUtility.singleLineHeight * 1.2f;
                propertyHeight += EditorGUIUtility.singleLineHeight * 1.2f;

                if ((SceneParameteredEvent.ParameterType)paramTypeProperty.enumValueIndex == SceneParameteredEvent.ParameterType.SCENE_PARAM)
                {
                    Rect popupPosition = new Rect(position.x, position.y + propertyOffset, position.width * 0.75f, EditorGUIUtility.singleLineHeight);

                    sceneVariablesSO = property.FindPropertyRelative("sceneVariablesSO");
                    if (sceneVariablesSO.objectReferenceValue == null)
                    {
                        EditorGUI.LabelField(popupPosition, "SceneVariablesSO is not assigned !");
                        EditorGUI.EndProperty();
                        return;
                    }
                    // Get the SceneVariablesSO
                    sceneVariablesObj = new SerializedObject(sceneVariablesSO.objectReferenceValue);
                    sceneVarContainer = sceneVariablesObj.targetObject as SceneVariablesSO;
                    if (sceneVarContainer == null)
                    {
                        EditorGUI.LabelField(popupPosition, "SceneVariablesSO is null !");
                        EditorGUI.EndProperty();
                        return;
                    }

                    List<SceneVar> sceneVarList = sceneVarContainer.NonEvents;
                    sceneVarUniqueIDP = property.FindPropertyRelative("paramUniqueID");
                    int sceneVarIndexSave = sceneVarContainer.GetIndexByUniqueID(sceneVarList, sceneVarUniqueIDP.intValue);
                    if (sceneVarIndexSave == -1) sceneVarIndexSave = 0;

                    // SceneVar choice popup
                    sceneVarIndex = EditorGUI.Popup(popupPosition, sceneVarIndexSave, sceneVarContainer.VarStrings(sceneVarList).ToArray());
                    if (sceneVarContainer.GetUniqueIDByIndex(sceneVarList, sceneVarIndex) == 0) sceneVarIndex = sceneVarIndexSave;
                    sceneVarUniqueIDP.intValue = sceneVarContainer.GetUniqueIDByIndex(sceneVarList, sceneVarIndex);
                    propertyHeight += EditorGUIUtility.singleLineHeight * 1.2f;

                    // Type label
                    Rect typeLabelPosition = new Rect(position.x + position.width * 0.77f, position.y + propertyOffset, position.width * 0.23f, EditorGUIUtility.singleLineHeight);
                    EditorGUI.LabelField(typeLabelPosition, sceneVarList[sceneVarIndex].type.ToString());
                }

                else if ((SceneParameteredEvent.ParameterType)paramTypeProperty.enumValueIndex == SceneParameteredEvent.ParameterType.CONDITION_PARAM)
                {
                    Rect conditionsPosition = new Rect(position.x, position.y + propertyOffset, position.width, EditorGUIUtility.singleLineHeight);
                    EditorGUI.PropertyField(conditionsPosition, conditionsProperty);
                    propertyHeight += EditorGUI.GetPropertyHeight(conditionsProperty) + EditorGUIUtility.singleLineHeight * 0.2f;
                }
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
