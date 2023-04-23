using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using System.Text;

namespace Dhs5.Utility.SceneCreation
{
    [CustomPropertyDrawer(typeof(AdvancedUnityEvent))]
    public class AdvancedUnityEventEditor : PropertyDrawer
    {
        SerializedProperty componentProperty;
        SerializedProperty tokenProperty;
        SerializedProperty actionProperty;

        int methodIndex;
        float propertyOffset;

        private bool ValidParameters(ParameterInfo[] parameters)
        {
            foreach (var param in parameters)
            {
                if (param.ParameterType != typeof(int)
                    && param.ParameterType != typeof(float)
                    && param.ParameterType != typeof(bool)
                    && param.ParameterType != typeof(string))
                    return false;
            }
            return true;
        }

        private string MethodName(MethodInfo method)
        {
            StringBuilder sb = new();
            sb.Append(method.Name);

            ParameterInfo[] parameters = method.GetParameters();
            if (parameters.Length > 0)
            {
                sb.Append(" (");
                for (int i = 0; i < parameters.Length - 1; i++)
                {
                    sb.Append(ParameterName(parameters[i]));
                    sb.Append(", ");
                }
                sb.Append(ParameterName(parameters[parameters.Length - 1]));
                sb.Append(")");
            }
            return sb.ToString();
        }

        private string ParameterName(ParameterInfo param)
        {
            StringBuilder sb = new();
            switch (param.ParameterType.Name)
            {
                case "String":
                    sb.Append("string");
                    break;
                case "Int32":
                    sb.Append("int");
                    break;
                case "Single":
                    sb.Append("float");
                    break;
                case "Boolean":
                    sb.Append("bool");
                    break;
            }
            sb.Append(" ");
            sb.Append(param.Name);

            return sb.ToString();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            propertyOffset = 0;

            actionProperty = property.FindPropertyRelative("action");
            tokenProperty = property.FindPropertyRelative("metadataToken");
            componentProperty = property.FindPropertyRelative("component");
            MethodInfo[] methods = componentProperty.serializedObject.targetObject.GetType().GetMethods();
            List<MethodInfo> methodInfos = new();
            List<string> methodNames = new();
            foreach (MethodInfo method in methods)
            {
                if (method.IsPublic && !method.IsStatic && !method.IsAbstract && !method.IsGenericMethod && !method.IsConstructor && !method.IsAssembly 
                    && !method.IsFamily && !method.ContainsGenericParameters && !method.IsSpecialName && method.ReturnType == typeof(void) 
                    && ValidParameters(method.GetParameters()))
                {
                    methodNames.Add(MethodName(method));
                    methodInfos.Add(method);
                }
            }
            methodIndex = methodInfos.FindIndex(m => m.MetadataToken == tokenProperty.intValue);
            if (methodIndex == -1) methodIndex = 0;

            EditorGUI.BeginProperty(position, label, property);

            Rect componentRect = new Rect(position.x, position.y + propertyOffset, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(componentRect, componentProperty);
            propertyOffset += EditorGUIUtility.singleLineHeight * 1.2f;

            Rect methodsRect = new Rect(position.x, position.y + propertyOffset, position.width, EditorGUIUtility.singleLineHeight);
            methodIndex = EditorGUI.Popup(methodsRect, methodIndex, methodNames.ToArray());
            propertyOffset += EditorGUIUtility.singleLineHeight * 1.2f;

            tokenProperty.intValue = methodInfos[methodIndex].MetadataToken;
            ParameterInfo[] parameters = methodInfos[methodIndex].GetParameters();
            object[] parameterValues = new object[parameters.Length];

            // ---------------
            for (int i = 0; i < parameters.Length; i++)
            {
                Rect valueRect = new Rect(position.x, position.y + propertyOffset, position.width, EditorGUIUtility.singleLineHeight);

                switch (parameters[i].ParameterType.Name)
                {
                    case nameof(System.Single):
                        parameterValues[i] = EditorGUI.FloatField(valueRect, parameters[i].Name, 0f);
                        break;
                    case nameof(System.Boolean):
                        parameterValues[i] = EditorGUI.Toggle(valueRect, parameters[i].Name, false);
                        break;
                    case nameof(System.Int32):
                        if (parameterValues[i] == null) parameterValues[i] = 0;
                        parameterValues[i] = EditorGUI.IntField(valueRect, parameters[i].Name, 0);
                        break;
                    case nameof(System.String):
                        parameterValues[i] = EditorGUI.TextField(valueRect, parameters[i].Name, " ");
                        break;
                }
                propertyOffset += EditorGUIUtility.singleLineHeight * 1.2f;
            }

            FieldInfo objField = property.serializedObject.targetObject.GetType().GetField(property.propertyPath);
            FieldInfo actionField = typeof(AdvancedUnityEvent).GetField("action");

            if (actionField != null && objField != null)
            {
                switch (parameters.Length)
                {
                    case 0:
                        Action action = (Action)methodInfos[methodIndex].CreateDelegate(typeof(Action), componentProperty.serializedObject.targetObject);
                        EventAction eventAction = new(action);
                        actionField.SetValue(objField.GetValue(property.serializedObject.targetObject), eventAction);
                        break;
                    case 1:
                        dynamic arg0 = parameterValues[0];
                        actionField.SetValue(objField.GetValue(property.serializedObject.targetObject), CreateAction(arg0, methodInfos[methodIndex], componentProperty.serializedObject.targetObject));
                        break;
                    case 2:
                        dynamic arg0_1 = parameterValues[0];
                        dynamic arg1_1 = parameterValues[0];
                        actionField.SetValue(objField.GetValue(property.serializedObject.targetObject), CreateAction(arg0_1, arg1_1, methodInfos[methodIndex], componentProperty.serializedObject.targetObject));
                        break;
                }
                
            }

            EditorGUI.EndProperty();
        }

        private EventAction<T1> CreateAction<T1>(T1 arg0, MethodInfo methodInfo, object target)
        {
            Action<T1> action = (Action<T1>)methodInfo.CreateDelegate(typeof(Action<T1>), target);
            return new EventAction<T1>(action, arg0);
        }
        private EventAction<T1, T2> CreateAction<T1, T2>(T1 arg0, T2 arg1, MethodInfo methodInfo, object target)
        {
            Action<T1, T2> action = (Action<T1, T2>)methodInfo.CreateDelegate(typeof(Action<T1, T2>), target);
            return new EventAction<T1, T2>(action, arg0, arg1);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 5;
        }
    }
}
