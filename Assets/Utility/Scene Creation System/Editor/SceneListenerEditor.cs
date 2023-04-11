using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dhs5.Utility.SceneCreation
{
    [CustomPropertyDrawer(typeof(SceneListener))]
    public class SceneListenerEditor : PropertyDrawer
    {
        SerializedProperty sceneVariablesSO;
        SerializedObject sceneVariablesObj;
        SerializedProperty sceneVars;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            sceneVariablesSO = property.FindPropertyRelative("sceneVariablesSO");
            sceneVariablesObj = new SerializedObject(sceneVariablesSO.objectReferenceValue);
            sceneVars = sceneVariablesObj.FindProperty("sceneVars");

            List<string> varIDs = new();
            GenericMenu menu = new();
            for (int i = 0; i < sceneVars.arraySize; i++)
            {
                varIDs.Add(sceneVars.GetArrayElementAtIndex(i).FindPropertyRelative("ID").stringValue);
                menu.AddItem(new GUIContent(varIDs[i]), false, null);
            }

            GUIContent dropdownContent = new(varIDs[0]);
            position = new Rect(position.x, position.y, position.width, position.height);
            if (EditorGUI.DropdownButton(position, dropdownContent, FocusType.Keyboard))
            {
                menu.ShowAsContext();
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * 3;
        }
    }
}
