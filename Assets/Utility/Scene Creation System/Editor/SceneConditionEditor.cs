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
        private SerializedProperty sceneVarUniqueID2P;

        int sceneVarIndex1 = 0;
        int sceneVarIndex2 = 0;

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
            sceneVarUniqueID1P = property.FindPropertyRelative("var1UniqueID");
            int sceneVarIndexSave1 = sceneVarContainer.GetIndexByUniqueID(sceneVarUniqueID1P.intValue);
            if (sceneVarIndexSave1 == -1) sceneVarIndexSave1 = 0;
            // SceneVar1 choice popup
            Rect popup1Position = new Rect(position.x, position.y, position.width * 0.35f, EditorGUIUtility.singleLineHeight);
            sceneVarIndex1 = EditorGUI.Popup(popup1Position, sceneVarIndexSave1, sceneVarContainer.IDs.ToArray());
            if (sceneVarContainer.GetUniqueIDByIndex(sceneVarIndex1) == 0) sceneVarIndex1 = sceneVarIndexSave1;
            sceneVarUniqueID1P.intValue = sceneVarContainer.GetUniqueIDByIndex(sceneVarIndex1);

            // Comparison operator
            Rect compPosition = new Rect(position.x + position.width * 0.36f, position.y, position.width * 0.28f, EditorGUIUtility.singleLineHeight);
            SceneVarType type = sceneVarContainer.sceneVars[sceneVarIndex1].type;
            switch (type)
            {
                case SceneVarType.BOOL:
                    EditorGUI.PropertyField(compPosition, property.FindPropertyRelative("boolComp"), new GUIContent(""));
                    conditionDescription = SceneCondition.BoolCompDescription((BoolComparison)property.FindPropertyRelative("boolComp").enumValueIndex);
                    break;
                case SceneVarType.INT:
                    EditorGUI.PropertyField(compPosition, property.FindPropertyRelative("intComp"), new GUIContent(""));
                    conditionDescription = SceneCondition.IntCompDescription((IntComparison)property.FindPropertyRelative("intComp").enumValueIndex);
                    break;
                case SceneVarType.FLOAT:
                    EditorGUI.PropertyField(compPosition, property.FindPropertyRelative("floatComp"), new GUIContent(""));
                    conditionDescription = SceneCondition.FloatCompDescription((FloatComparison)property.FindPropertyRelative("floatComp").enumValueIndex);
                    break;
                case SceneVarType.STRING:
                    EditorGUI.PropertyField(compPosition, property.FindPropertyRelative("stringComp"), new GUIContent(""));
                    conditionDescription = SceneCondition.StringCompDescription((StringComparison)property.FindPropertyRelative("stringComp").enumValueIndex);
                    break;
            }

            // SceneVar 2
            sceneVarUniqueID2P = property.FindPropertyRelative("var2UniqueID");
            int sceneVarIndexSave2 = sceneVarContainer.GetIndexByUniqueID(sceneVarUniqueID2P.intValue);
            if (sceneVarIndexSave2 == -1) sceneVarIndexSave2 = 0;
            // SceneVar1 choice popup
            Rect popup2Position = new Rect(position.x + position.width * 0.65f, position.y, position.width * 0.35f, EditorGUIUtility.singleLineHeight);
            sceneVarIndex2 = EditorGUI.Popup(popup2Position, sceneVarIndexSave2, sceneVarContainer.IDs.ToArray());
            if (sceneVarContainer.GetUniqueIDByIndex(sceneVarIndex2) == 0) sceneVarIndex2 = sceneVarIndexSave2;
            sceneVarUniqueID2P.intValue = sceneVarContainer.GetUniqueIDByIndex(sceneVarIndex2);

            // Description labels
            Rect label1Position = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width * 0.35f, EditorGUIUtility.singleLineHeight);
            Rect label2Position = new Rect(position.x + position.width * 0.36f, position.y + EditorGUIUtility.singleLineHeight, position.width * 0.28f, EditorGUIUtility.singleLineHeight);
            Rect label3Position = new Rect(position.x + position.width * 0.65f, position.y + EditorGUIUtility.singleLineHeight, position.width * 0.35f, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(label1Position, type.ToString(), EditorStyles.miniLabel);
            EditorGUI.LabelField(label2Position, conditionDescription, EditorStyles.miniLabel);
            EditorGUI.LabelField(label3Position, sceneVarContainer.sceneVars[sceneVarIndex2].type.ToString(), EditorStyles.miniLabel);

            // Logical Operator property
            Rect opPosition = new Rect(position.x + position.width * 0.75f, position.y + EditorGUIUtility.singleLineHeight * 1.9f, position.width * 0.25f, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(opPosition, property.FindPropertyRelative("logicOperator"), new GUIContent(""));

            // End
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2.8f;
        }
    }
}
