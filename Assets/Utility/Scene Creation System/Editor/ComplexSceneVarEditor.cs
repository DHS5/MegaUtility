using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dhs5.Utility.SceneCreation
{
    [CustomPropertyDrawer(typeof(ComplexSceneVar))]
    public class ComplexSceneVarEditor : PropertyDrawer
    {
        private SceneVariablesSO container;
        
        private SerializedProperty uniqueIDProperty;
        private SerializedProperty idProperty;
        private SerializedProperty typeProperty;

        private float propertyOffset;
        private float propertyHeight;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            propertyOffset = 0;
            propertyHeight = 0;

            container = property.serializedObject.targetObject as SceneVariablesSO;
            
            uniqueIDProperty = property.FindPropertyRelative("uniqueID");
            idProperty = property.FindPropertyRelative("ID");
            typeProperty = property.FindPropertyRelative("type");
            
            EditorGUI.BeginProperty(position, label, property);

            SceneVar var = container[uniqueIDProperty.intValue];

            Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, 
                property.isExpanded ? "" : var != null ? var.ToString() : "No ID yet");
            if (property.isExpanded)
            {
                // Name
                Rect nameRect = new Rect(position.x, position.y,
                    position.width / 2, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(nameRect, var != null ? var.ID : "No ID yet");
                // Unique ID
                Rect uniqueIDRect = new Rect(position.x + position.width * 0.6f, position.y, 
                    position.width / 2, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(uniqueIDRect, "Unique ID : " + uniqueIDProperty.intValue.ToString());

                if (uniqueIDProperty.intValue == 0)
                {
                    if (container != null)
                    {
                        uniqueIDProperty.intValue = container.GenerateUniqueID();
                    }
                    else
                    {
                        Debug.LogError("Can't generate unique ID for SceneVar that is not on a SceneVariablesSO");
                    }
                }

                propertyOffset += EditorGUIUtility.singleLineHeight * 1.5f;
                propertyHeight += EditorGUIUtility.singleLineHeight * 1.5f;
                
                // ID
                Rect idLabelRect = new Rect(position.x, position.y + propertyOffset, 15, EditorGUIUtility.singleLineHeight);
                Rect idRect = new Rect(position.x + 20, position.y + propertyOffset, position.width * 0.75f - 20, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(idLabelRect, "ID");
                EditorGUI.PropertyField(idRect, idProperty, new GUIContent(""));

                // Type
                Rect typeRect = new Rect(position.x + position.width * 0.76f, position.y + propertyOffset, position.width * 0.24f,
                    EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(typeRect, typeProperty, new GUIContent(""));
                propertyOffset += EditorGUIUtility.singleLineHeight * 1.2f;
                propertyHeight += EditorGUIUtility.singleLineHeight * 1.2f;

                // Value
                ComplexSceneVarType type = (ComplexSceneVarType)typeProperty.enumValueIndex;
                Rect valueRect = new Rect(position.x, position.y + propertyOffset, position.width, EditorGUIUtility.singleLineHeight);

                SerializedProperty valueProperty = null;
                switch (type)
                {
                    case ComplexSceneVarType.CONDITION:
                        valueProperty = property.FindPropertyRelative("conditions");
                        break;
                    case ComplexSceneVarType.TOTAL_INT:
                        //valueProperty = property.FindPropertyRelative("intTotals");
                        break;
                    case ComplexSceneVarType.TOTAL_FLOAT:
                        //valueProperty = property.FindPropertyRelative("floatTotals");
                        break;
                    case ComplexSceneVarType.SENTENCE:
                        //valueProperty = property.FindPropertyRelative("sentences");
                        break;
                }
                if (valueProperty != null)
                {
                    EditorGUI.PropertyField(valueRect, valueProperty, new GUIContent(""));
                    propertyHeight += EditorGUI.GetPropertyHeight(valueProperty);
                }

                propertyHeight += EditorGUIUtility.singleLineHeight * 0.2f;
            }

            property.FindPropertyRelative("propertyHeight").floatValue = propertyHeight;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return property.isExpanded ? property.FindPropertyRelative("propertyHeight").floatValue : EditorGUIUtility.singleLineHeight;
        }
    }
}
