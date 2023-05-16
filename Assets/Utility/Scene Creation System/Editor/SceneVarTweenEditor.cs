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

        private SerializedProperty canBeStaticP;
        private SerializedProperty isStaticP;

        int sceneVarIndex = 0;
        int sceneVarIndexSave = 0;

        float propertyOffset;
        float propertyHeight;
        bool isCondition;

        bool emptyLabel;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            emptyLabel = string.IsNullOrEmpty(label.text);

            sceneVarIndex = 0;

            propertyOffset = 0f;
            propertyHeight = 0f;
            isCondition = false;

            canBeStaticP = property.FindPropertyRelative("canBeStatic");
            isStaticP = property.FindPropertyRelative("isStatic");

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

            SceneVarType type = (SceneVarType)property.FindPropertyRelative("type").enumValueIndex;
            List<SceneVar> sceneVarList = sceneVarContainer.GetListByType(type, type == SceneVarType.INT);
            // Clean list of dependency cycles
            int forbiddenUID = property.FindPropertyRelative("forbiddenUID").intValue;
            if (forbiddenUID != -1)
            {
                sceneVarList = sceneVarContainer.CleanListOfCycleDependencies(sceneVarList, forbiddenUID);
            }

            sceneVarUniqueIDP = property.FindPropertyRelative("sceneVarUniqueID");
            sceneVarIndexSave = sceneVarContainer.GetIndexByUniqueID(sceneVarList, sceneVarUniqueIDP.intValue);
            if (sceneVarIndexSave == -1) sceneVarIndexSave = 0;

            propertyOffset += EditorGUIUtility.singleLineHeight * 0.25f;
            propertyHeight += EditorGUIUtility.singleLineHeight * 0.25f;

            //if (type == SceneVarType.BOOL && (!canBeStaticP.boolValue || !isStaticP.boolValue))
            //{
            //    SerializedProperty boolTypeProperty = property.FindPropertyRelative("boolType");
            //
            //    Rect boolTypePosition = new Rect(position.x + position.width * 0.38f, position.y + EditorGUIUtility.singleLineHeight * 0.25f, position.width * 0.3f, EditorGUIUtility.singleLineHeight);
            //    EditorGUI.PropertyField(boolTypePosition, boolTypeProperty, new GUIContent(""));
            //    propertyOffset += EditorGUIUtility.singleLineHeight * 1.25f;
            //    propertyHeight += EditorGUIUtility.singleLineHeight * 1.25f;
            //
            //    SceneVarTween.BoolType boolType = (SceneVarTween.BoolType)boolTypeProperty.enumValueIndex;
            //    if (boolType == SceneVarTween.BoolType.CONDITION)
            //    {
            //        string goodText = label.text;
            //        isCondition = true;
            //        SerializedProperty conditionProperty = property.FindPropertyRelative("sceneConditions");
            //        Rect conditionPosition = new Rect(position.x + 16, position.y + propertyOffset, position.width - 20, EditorGUIUtility.singleLineHeight);
            //        EditorGUI.PropertyField(conditionPosition, conditionProperty);
            //        propertyHeight += EditorGUI.GetPropertyHeight(conditionProperty);
            //        label.text = goodText;
            //    }
            //    else
            //    {
            //        propertyHeight += EditorGUIUtility.singleLineHeight * 1.2f;
            //    }
            //    property.FindPropertyRelative("propertyHeight").floatValue = propertyHeight;
            //}
            //else
            //{
            //    property.FindPropertyRelative("propertyHeight").floatValue = EditorGUIUtility.singleLineHeight * 1.5f;
            //}

            if (!canBeStaticP.boolValue)
            {
                // Label
                Rect labelPosition = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 0.25f, position.width * 0.3f, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(labelPosition, label);

                if (!isCondition)
                {
                    // SceneVar choice popup
                    Rect popupPosition = new Rect(position.x + (emptyLabel ? 0 : position.width * 0.32f), position.y + propertyOffset, position.width * (emptyLabel ? 0.84f : 0.52f), EditorGUIUtility.singleLineHeight);
                    sceneVarIndex = EditorGUI.Popup(popupPosition, sceneVarIndexSave, sceneVarContainer.VarStrings(sceneVarList).ToArray());
                    if (sceneVarContainer.GetUniqueIDByIndex(sceneVarList, sceneVarIndex) == 0) sceneVarIndex = sceneVarIndexSave;
                    sceneVarUniqueIDP.intValue = sceneVarContainer.GetUniqueIDByIndex(sceneVarList, sceneVarIndex);
                }

                // Label
                Rect typePosition = new Rect(position.x + position.width * 0.85f, position.y + EditorGUIUtility.singleLineHeight * 0.25f, position.width * 0.18f, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(typePosition, sceneVarContainer[sceneVarUniqueIDP.intValue].type.ToString());
            }
            else
            {
                // Label
                Rect labelPosition = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 0.25f, position.width * 0.25f, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(labelPosition, label);

                // SceneVar choice popup
                Rect popupPosition = new Rect(position.x + (emptyLabel ? 0 : position.width * 0.27f), position.y + propertyOffset, position.width * (emptyLabel ? 0.72f : 0.45f), EditorGUIUtility.singleLineHeight);
                if (!isStaticP.boolValue && !isCondition)
                {
                    sceneVarIndex = EditorGUI.Popup(popupPosition, sceneVarIndexSave, sceneVarContainer.VarStrings(sceneVarList).ToArray());
                    if (sceneVarContainer.GetUniqueIDByIndex(sceneVarList, sceneVarIndex) == 0) sceneVarIndex = sceneVarIndexSave;
                    sceneVarUniqueIDP.intValue = sceneVarContainer.GetUniqueIDByIndex(sceneVarList, sceneVarIndex);
                }
                else if (isStaticP.boolValue)
                {
                    string typeStr = "";
                    switch (type)
                    {
                        case SceneVarType.BOOL:
                            typeStr = "bool";
                            break;
                        case SceneVarType.INT:
                            typeStr = "int";
                            break;
                        case SceneVarType.FLOAT:
                            typeStr = "float";
                            break;
                        case SceneVarType.STRING:
                            typeStr = "string";
                            break;
                    }
                    EditorGUI.PropertyField(popupPosition, property.FindPropertyRelative(typeStr + "Value"), new GUIContent(""));
                }

                // Label
                Rect typePosition = new Rect(position.x + position.width * 0.73f, position.y + EditorGUIUtility.singleLineHeight * 0.25f, position.width * 0.1f, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(typePosition, sceneVarContainer[sceneVarUniqueIDP.intValue].type.ToString());

                // Static toggle
                Rect staticRect = new Rect(position.x + position.width * 0.84f, position.y + EditorGUIUtility.singleLineHeight * 0.25f, position.width * 0.15f, EditorGUIUtility.singleLineHeight);
                isStaticP.boolValue = EditorGUI.ToggleLeft(staticRect, "Static", isStaticP.boolValue);
            }

            // End
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 1.5f;// property.FindPropertyRelative("propertyHeight").floatValue;
        }
    }
}
