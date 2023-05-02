using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dhs5.Utility.SceneCreation
{
    [CustomPropertyDrawer(typeof(SceneVar))]
    public class SceneVarEditor : PropertyDrawer
    {
        private SceneVariablesSO container;
        
        private SerializedProperty uniqueIDProperty;
        private SerializedProperty idProperty;
        private SerializedProperty typeProperty;
        private SerializedProperty staticProperty;

        private SerializedProperty hasMinProperty;
        private SerializedProperty hasMaxProperty;

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
            staticProperty = property.FindPropertyRelative("isStatic");
            
            EditorGUI.BeginProperty(position, label, property);

            Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, 
                property.isExpanded ? "" : container[uniqueIDProperty.intValue].ToString());
            if (property.isExpanded)
            {
                // Name
                Rect nameRect = new Rect(position.x, position.y,
                    position.width / 2, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(nameRect, container[uniqueIDProperty.intValue].ID);
                // Unique ID
                Rect uniqueIDRect = new Rect(position.x + position.width * 0.6f, position.y, 
                    position.width / 2, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(uniqueIDRect, "Unique ID : " + uniqueIDProperty.intValue.ToString());

                if (uniqueIDProperty.intValue == 0)
                {
                    if (container != null)
                        uniqueIDProperty.intValue = container.GenerateUniqueID();
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
                Rect valueLabelRect = new Rect(position.x, position.y + propertyOffset, 75, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(valueLabelRect, "Initial Value");
                Rect valueRect = new Rect(position.x + 75, position.y + propertyOffset, position.width * 0.75f - 75, EditorGUIUtility.singleLineHeight);

                SceneVarType type = (SceneVarType)typeProperty.enumValueIndex;
                switch (type)
                {
                    case SceneVarType.BOOL:
                        EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("boolValue"), new GUIContent(""));
                        break;
                    case SceneVarType.INT:
                        EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("intValue"), new GUIContent(""));
                        break;
                    case SceneVarType.FLOAT:
                        EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("floatValue"), new GUIContent(""));
                        break;
                    case SceneVarType.STRING:
                        EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("stringValue"), new GUIContent(""));
                        break;
                    case SceneVarType.EVENT:
                        EditorGUI.EndProperty();
                        return;
                }

                // Static
                Rect staticRect = new Rect(position.x + position.width * 0.76f, position.y + propertyOffset, position.width * 0.24f,
                    EditorGUIUtility.singleLineHeight);
                staticProperty.boolValue = EditorGUI.ToggleLeft(staticRect, "Static", staticProperty.boolValue);
                propertyOffset += EditorGUIUtility.singleLineHeight * 1.2f;
                propertyHeight += EditorGUIUtility.singleLineHeight * 1.2f;

                if (type == SceneVarType.INT || type == SceneVarType.FLOAT)
                {
                    hasMinProperty = property.FindPropertyRelative("hasMin");
                    hasMaxProperty = property.FindPropertyRelative("hasMax");

                    Rect hasMinRect = new Rect(position.x, position.y + propertyOffset, position.width * 0.12f, EditorGUIUtility.singleLineHeight);
                    Rect hasMaxRect = new Rect(position.x + position.width * 0.5f, position.y + propertyOffset, position.width * 0.12f, EditorGUIUtility.singleLineHeight);
                    hasMinProperty.boolValue = EditorGUI.ToggleLeft(hasMinRect, new GUIContent("Min"), hasMinProperty.boolValue);
                    hasMaxProperty.boolValue = EditorGUI.ToggleLeft(hasMaxRect, new GUIContent("Max"), hasMaxProperty.boolValue);

                    if (hasMinProperty.boolValue)
                    {
                        Rect minRect = new Rect(position.x + position.width * 0.13f, position.y + propertyOffset, position.width * 0.36f, EditorGUIUtility.singleLineHeight);

                        if (type == SceneVarType.INT)
                        {
                            EditorGUI.PropertyField(minRect, property.FindPropertyRelative("minInt"), new GUIContent(""));
                        }
                        else if (type == SceneVarType.FLOAT)
                        {
                            EditorGUI.PropertyField(minRect, property.FindPropertyRelative("minFloat"), new GUIContent(""));
                        }
                    }
                    if (hasMaxProperty.boolValue)
                    {
                        Rect maxRect = new Rect(position.x + position.width * 0.63f, position.y + propertyOffset, position.width * 0.36f, EditorGUIUtility.singleLineHeight);

                        if (type == SceneVarType.INT)
                        {
                            EditorGUI.PropertyField(maxRect, property.FindPropertyRelative("maxInt"), new GUIContent(""));
                        }
                        else if (type == SceneVarType.FLOAT)
                        {
                            EditorGUI.PropertyField(maxRect, property.FindPropertyRelative("maxFloat"), new GUIContent(""));
                        }
                    }

                    propertyOffset += EditorGUIUtility.singleLineHeight;
                    propertyHeight += EditorGUIUtility.singleLineHeight;
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
