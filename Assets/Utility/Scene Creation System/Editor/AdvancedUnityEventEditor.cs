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
                        property.FindPropertyRelative("float" + i).floatValue = EditorGUI.FloatField(valueRect, parameters[i].Name, property.FindPropertyRelative("float"+i).floatValue);
                        parameterValues[i] = property.FindPropertyRelative("float" + i).floatValue;
                        break;
                    case nameof(System.Boolean):
                        property.FindPropertyRelative("bool" + i).boolValue = EditorGUI.Toggle(valueRect, parameters[i].Name, property.FindPropertyRelative("bool" + i).boolValue);
                        parameterValues[i] = property.FindPropertyRelative("bool" + i).boolValue;
                        break;
                    case nameof(System.Int32):
                        property.FindPropertyRelative("int" + i).intValue = EditorGUI.IntField(valueRect, parameters[i].Name, property.FindPropertyRelative("int" + i).intValue);
                        parameterValues[i] = property.FindPropertyRelative("int" + i).intValue;
                        break;
                    case nameof(System.String):
                        property.FindPropertyRelative("string" + i).stringValue = EditorGUI.TextField(valueRect, parameters[i].Name, property.FindPropertyRelative("string" + i).stringValue);
                        parameterValues[i] = property.FindPropertyRelative("string" + i).stringValue;
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
                        dynamic arg1_1 = parameterValues[1];
                        actionField.SetValue(objField.GetValue(property.serializedObject.targetObject), CreateAction(arg0_1, arg1_1, methodInfos[methodIndex], componentProperty.serializedObject.targetObject));
                        break;
                    case 3:
                        dynamic arg0_2 = parameterValues[0];
                        dynamic arg1_2 = parameterValues[1];
                        dynamic arg2_2 = parameterValues[2];
                        actionField.SetValue(objField.GetValue(property.serializedObject.targetObject), CreateAction(arg0_2, arg1_2, arg2_2, methodInfos[methodIndex], componentProperty.serializedObject.targetObject));
                        break;
                    case 4:
                        dynamic arg0_3 = parameterValues[0];
                        dynamic arg1_3 = parameterValues[1];
                        dynamic arg2_3 = parameterValues[2];
                        dynamic arg3_3 = parameterValues[3];
                        actionField.SetValue(objField.GetValue(property.serializedObject.targetObject), CreateAction(arg0_3, arg1_3, arg2_3, arg3_3, methodInfos[methodIndex], componentProperty.serializedObject.targetObject));
                        break;
                    case 5:
                        dynamic arg0_4 = parameterValues[0];
                        dynamic arg1_4 = parameterValues[1];
                        dynamic arg2_4 = parameterValues[2];
                        dynamic arg3_4 = parameterValues[3];
                        dynamic arg4_4 = parameterValues[4];
                        actionField.SetValue(objField.GetValue(property.serializedObject.targetObject), CreateAction(arg0_4, arg1_4, arg2_4, arg3_4, arg4_4, methodInfos[methodIndex], componentProperty.serializedObject.targetObject));
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
        private EventAction<T1, T2, T3> CreateAction<T1, T2, T3>(T1 arg0, T2 arg1, T3 arg2, MethodInfo methodInfo, object target)
        {
            Action<T1, T2, T3> action = (Action<T1, T2, T3>)methodInfo.CreateDelegate(typeof(Action<T1, T2, T3>), target);
            return new EventAction<T1, T2, T3>(action, arg0, arg1, arg2);
        }
        private EventAction<T1, T2, T3, T4> CreateAction<T1, T2, T3, T4>(T1 arg0, T2 arg1, T3 arg2, T4 arg3, MethodInfo methodInfo, object target)
        {
            Action<T1, T2, T3, T4> action = (Action<T1, T2, T3, T4>)methodInfo.CreateDelegate(typeof(Action<T1, T2, T3, T4>), target);
            return new EventAction<T1, T2, T3, T4>(action, arg0, arg1, arg2, arg3);
        }
        private EventAction<T1, T2, T3, T4, T5> CreateAction<T1, T2, T3, T4, T5>(T1 arg0, T2 arg1, T3 arg2, T4 arg3, T5 arg4, MethodInfo methodInfo, object target)
        {
            Action<T1, T2, T3, T4, T5> action = (Action<T1, T2, T3, T4, T5>)methodInfo.CreateDelegate(typeof(Action<T1, T2, T3, T4, T5>), target);
            return new EventAction<T1, T2, T3, T4, T5>(action, arg0, arg1, arg2, arg3, arg4);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 10;
        }
    }
}
