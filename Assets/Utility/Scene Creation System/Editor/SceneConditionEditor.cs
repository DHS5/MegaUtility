using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dhs5.Utility.SceneCreation
{
    [CustomPropertyDrawer(typeof(SceneCondition))]
    public class SceneConditionEditor : PropertyDrawer
    {
        SerializedProperty sceneVariablesSO;
        SerializedObject sceneVariablesObj;
        SceneVariablesSO sceneVarContainer;

        private SerializedProperty sceneVarUniqueID1P;

        int sceneVarIndex1 = 0;

        GUIContent empty = new GUIContent("");

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string conditionDescription = "";

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

            // SceneVar 1
            List<SceneVar> sceneVarList1 = sceneVarContainer.Conditionable;
            // Clean list of dependency cycles
            int forbiddenUID = property.FindPropertyRelative("forbiddenUID").intValue;
            if (forbiddenUID != -1)
            {
                sceneVarList1 = sceneVarContainer.CleanListOfCycleDependencies(sceneVarList1, forbiddenUID);
            }

            sceneVarUniqueID1P = property.FindPropertyRelative("var1UniqueID");
            int sceneVarIndexSave1 = sceneVarContainer.GetIndexByUniqueID(sceneVarList1, sceneVarUniqueID1P.intValue);
            if (sceneVarIndexSave1 == -1) sceneVarIndexSave1 = 0;
            // SceneVar1 choice popup
            Rect popup1Position = new Rect(position.x, position.y, position.width * 0.75f, EditorGUIUtility.singleLineHeight);
            sceneVarIndex1 = EditorGUI.Popup(popup1Position, sceneVarIndexSave1, sceneVarContainer.VarStrings(sceneVarList1).ToArray());
            if (sceneVarContainer.GetUniqueIDByIndex(sceneVarList1, sceneVarIndex1) == 0) sceneVarIndex1 = sceneVarIndexSave1;
            sceneVarUniqueID1P.intValue = sceneVarContainer.GetUniqueIDByIndex(sceneVarList1, sceneVarIndex1);

            // Comparison operator
            Rect compPosition = new Rect(position.x + position.width * 0.76f, position.y + EditorGUIUtility.singleLineHeight * 0.5f, position.width * 0.24f, EditorGUIUtility.singleLineHeight);
            SceneVarType type = sceneVarList1[sceneVarIndex1].type;
            property.FindPropertyRelative("var2Type").enumValueIndex = (int)type;

            switch (type)
            {
                case SceneVarType.BOOL:
                    EditorGUI.PropertyField(compPosition, property.FindPropertyRelative("boolComp"), empty);
                    conditionDescription = SceneCondition.BoolCompDescription((BoolComparison)property.FindPropertyRelative("boolComp").enumValueIndex);
                    break;
                case SceneVarType.INT:
                    EditorGUI.PropertyField(compPosition, property.FindPropertyRelative("intComp"), empty);
                    conditionDescription = SceneCondition.IntCompDescription((IntComparison)property.FindPropertyRelative("intComp").enumValueIndex);
                    break;
                case SceneVarType.FLOAT:
                    EditorGUI.PropertyField(compPosition, property.FindPropertyRelative("floatComp"), empty);
                    conditionDescription = SceneCondition.FloatCompDescription((FloatComparison)property.FindPropertyRelative("floatComp").enumValueIndex);
                    break;
                case SceneVarType.STRING:
                    EditorGUI.PropertyField(compPosition, property.FindPropertyRelative("stringComp"), empty);
                    conditionDescription = SceneCondition.StringCompDescription((StringComparison)property.FindPropertyRelative("stringComp").enumValueIndex);
                    break;
                default:
                    EditorGUI.EndProperty();
                    return;
            }

            Rect var2Position = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 1.25f, position.width * 0.75f, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(var2Position, property.FindPropertyRelative("SceneVar2"), empty);

            /*
            // SceneVar 2
            List<SceneVar> sceneVarList2 = sceneVarContainer.GetListByType(type);
            sceneVarUniqueID2P = property.FindPropertyRelative("var2UniqueID");
            int sceneVarIndexSave2 = sceneVarContainer.GetIndexByUniqueID(sceneVarList2, sceneVarUniqueID2P.intValue);
            if (sceneVarIndexSave2 == -1) sceneVarIndexSave2 = 0;
            // SceneVar1 choice popup
            Rect popup2Position = new Rect(position.x + position.width * 0.65f, position.y, position.width * 0.35f, EditorGUIUtility.singleLineHeight);
            sceneVarIndex2 = EditorGUI.Popup(popup2Position, sceneVarIndexSave2, sceneVarContainer.VarStrings(sceneVarList2).ToArray());
            if (sceneVarContainer.GetUniqueIDByIndex(sceneVarList2, sceneVarIndex2) == 0) sceneVarIndex2 = sceneVarIndexSave2;
            sceneVarUniqueID2P.intValue = sceneVarContainer.GetUniqueIDByIndex(sceneVarList2, sceneVarIndex2);

            // Description labels
            Rect label1Position = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width * 0.35f, EditorGUIUtility.singleLineHeight);
            Rect label2Position = new Rect(position.x + position.width * 0.36f, position.y + EditorGUIUtility.singleLineHeight, position.width * 0.28f, EditorGUIUtility.singleLineHeight);
            Rect label3Position = new Rect(position.x + position.width * 0.65f, position.y + EditorGUIUtility.singleLineHeight, position.width * 0.35f, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(label1Position, type.ToString(), EditorStyles.miniLabel);
            EditorGUI.LabelField(label2Position, conditionDescription, EditorStyles.miniLabel);
            EditorGUI.LabelField(label3Position, sceneVarList2[sceneVarIndex2].type.ToString(), EditorStyles.miniLabel);
            */

            // Logical Operator property
            Rect opPosition = new Rect(position.x + position.width * 0.8f, position.y + EditorGUIUtility.singleLineHeight * 1.9f, position.width * 0.2f, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(opPosition, property.FindPropertyRelative("logicOperator"), empty);

            // End
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2.8f;
        }
    }
}
