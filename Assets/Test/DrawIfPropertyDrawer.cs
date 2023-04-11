using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Dhs5.Utility;

namespace Dhs5.Test
{
    [CustomPropertyDrawer(typeof(DrawIfAttribute))]
    public class DrawIfPropertyDrawer : PropertyDrawer
    {
        DrawIfAttribute drawIfAttribute;
        bool condition;

        float propertyHeight;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            drawIfAttribute = attribute as DrawIfAttribute;
            SerializedProperty boolProperty = property.serializedObject.FindProperty(drawIfAttribute.PropertyName);
            object obj = null;

            if (boolProperty == null)
            {
                obj = property.GetParentField(drawIfAttribute.PropertyName);
            }

            propertyHeight = base.GetPropertyHeight(property, label);

            if ((boolProperty != null && boolProperty.type == "bool") || (obj != null && obj.GetType() == typeof(bool)))
            {
                condition = boolProperty != null ? boolProperty.boolValue : (bool)obj;
                if (drawIfAttribute.Reverse) condition = !condition;

                EditorGUI.BeginProperty(position, label, property);

                if (condition)
                {
                    EditorGUI.PropertyField(position, property, label, true);
                }
                else
                {
                    if (drawIfAttribute.HidingType == HidingType.READ_ONLY)
                    {
                        GUI.enabled = false;
                        EditorGUI.PropertyField(position, property, label, true);
                        GUI.enabled = true;
                    }
                    else
                    {
                        propertyHeight = 0f;
                    }
                }

                EditorGUI.EndProperty();
            }
            else
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return propertyHeight;
        }
    }
}
