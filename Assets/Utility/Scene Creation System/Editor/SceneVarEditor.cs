using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SceneCreation
{
    [CustomPropertyDrawer(typeof(SceneVar))]
    public class SceneVarEditor : PropertyDrawer
    {
        private SceneVariablesSO container;
        
        private SerializedProperty uniqueIDProperty;
        private SerializedProperty idProperty;
        private SerializedProperty typeProperty;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            container = property.serializedObject.targetObject as SceneVariablesSO;
            
            uniqueIDProperty = property.FindPropertyRelative("uniqueID");
            idProperty = property.FindPropertyRelative("ID");
            typeProperty = property.FindPropertyRelative("type");
            
            EditorGUI.BeginProperty(position, label, property);

            Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, idProperty.stringValue + " : " + (SceneVarType)typeProperty.enumValueIndex);
            if (property.isExpanded)
            {
                // Unique ID
                Rect uniqueIDRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, 
                    position.width / 2, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(uniqueIDRect, "Unique ID : " + uniqueIDProperty.intValue.ToString());
                if (uniqueIDProperty.intValue == 0)
                {
                    Rect generateIDRect = new Rect(position.x + position.width / 2, position.y + EditorGUIUtility.singleLineHeight, 
                        position.width / 2, EditorGUIUtility.singleLineHeight);
                    if (GUI.Button(generateIDRect, "Generate ID"))
                    {
                        if (container != null)
                            uniqueIDProperty.intValue = container.GenerateUniqueID();
                        else
                        {
                            Debug.LogError("Can't generate unique ID for SceneVar that is not on a SceneVariablesSO");
                        }
                    }
                }
                
                // ID
                Rect idRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 3, 
                    position.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(idRect, idProperty);
                // Type
                Rect typeRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 4, position.width,
                    EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(typeRect, typeProperty);
                // Value
                Rect valueRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 5,
                    position.width, EditorGUIUtility.singleLineHeight);

                switch ((SceneVarType)typeProperty.enumValueIndex)
                {
                    case SceneVarType.BOOL:
                        EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("boolValue"), new GUIContent("Initial Value"));
                        break;
                    case SceneVarType.INT:
                        EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("intValue"), new GUIContent("Initial Value"));
                        break;
                    case SceneVarType.FLOAT:
                        EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("floatValue"), new GUIContent("Initial Value"));
                        break;
                    case SceneVarType.STRING:
                        EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("stringValue"), new GUIContent("Initial Value"));
                        break;
                }
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return property.isExpanded ? EditorGUIUtility.singleLineHeight * 6.5f : EditorGUIUtility.singleLineHeight;
        }
    }
}
