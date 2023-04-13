using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Dhs5.Utility.SceneCreation
{
    #region Enums
    [Serializable]
    public enum SceneVarType
    {
        BOOL, INT, FLOAT, STRING
    }

    [Serializable]
    public enum BoolOperation
    {
        SET, INVERSE
    }
    [Serializable]
    public enum BoolComparison
    {
        EQUAL, DIFF
    }
    [Serializable]
    public enum IntOperation
    {
        SET, ADD
    }
    [Serializable]
    public enum IntComparison
    {
        EQUAL, DIFF, SUP, INF, SUP_EQUAL, INF_EQUAL
    }
    [Serializable]
    public enum FloatOperation
    {
        SET, ADD
    }
    [Serializable]
    public enum FloatComparison
    {
        EQUAL, DIFF, SUP, INF, SUP_EQUAL, INF_EQUAL
    }
    [Serializable]
    public enum StringOperation
    {
        SET, APPEND, REMOVE
    }
    [Serializable]
    public enum StringComparison
    {
        EQUAL, DIFF, CONTAINS, CONTAINED, NULL_EMPTY
    }
    #endregion

    public static class SceneState
    {
        private static Dictionary<int, SceneVar> SceneVariables = new();

        #region Private Utility functions
        private static void Clear()
        {
            SceneVariables.Clear();
        }
        private static void AddVar(SceneVar variable)
        {
            SceneVariables[variable.uniqueID] = variable;
            ChangedVar(variable.uniqueID);
        }
        private static void ChangedVar(int varUniqueID)
        {
            if (SceneVariables.ContainsKey(varUniqueID))
            {
                SceneEventManager.TriggerEvent(varUniqueID, SceneVariables[varUniqueID]);
            }
        }
        #endregion

        #region Public accessors
        public static object GetObjectValue(int varUniqueID)
        {
            if (SceneVariables.ContainsKey(varUniqueID))
                return SceneVariables[varUniqueID].Value;
            IncorrectID(varUniqueID);
            return null;
        }
        public static bool TryGetBoolValue(int varUniqueID, out bool value)
        {
            value = false;
            if (SceneVariables.ContainsKey(varUniqueID))
            {
                SceneVar sceneVar = SceneVariables[varUniqueID];
                if (sceneVar.type == SceneVarType.BOOL)
                {
                    value = sceneVar.boolValue;
                    return true;
                }
                IncorrectType(varUniqueID, SceneVarType.BOOL);
                return false;
            }
            IncorrectID(varUniqueID);
            return false;
        }
        public static bool TryGetIntValue(int varUniqueID, out int value)
        {
            value = 0;
            if (SceneVariables.ContainsKey(varUniqueID))
            {
                SceneVar sceneVar = SceneVariables[varUniqueID];
                if (sceneVar.type == SceneVarType.INT)
                {
                    value = sceneVar.intValue;
                    return true;
                }
                IncorrectType(varUniqueID, SceneVarType.INT);
                return false;
            }
            IncorrectID(varUniqueID);
            return false;
        }
        public static bool TryGetFloatValue(int varUniqueID, out float value)
        {
            value = 0f;
            if (SceneVariables.ContainsKey(varUniqueID))
            {
                SceneVar sceneVar = SceneVariables[varUniqueID];
                if (sceneVar.type == SceneVarType.FLOAT)
                {
                    value = sceneVar.floatValue;
                    return true;
                }
                IncorrectType(varUniqueID, SceneVarType.FLOAT);
                return false;
            }
            IncorrectID(varUniqueID);
            return false;
        }
        public static bool TryGetStringValue(int varUniqueID, out string value)
        {
            value = null;
            if (SceneVariables.ContainsKey(varUniqueID))
            {
                SceneVar sceneVar = SceneVariables[varUniqueID];
                if (sceneVar.type == SceneVarType.STRING)
                {
                    value = sceneVar.stringValue;
                    return true;
                }
                IncorrectType(varUniqueID, SceneVarType.STRING);
                return false;
            }
            IncorrectID(varUniqueID);
            return false;
        }
        #endregion

        #region Public setters
        public static void SetSceneVars(SceneVariablesSO sceneVariablesSO)
        {
            Clear();
            if (sceneVariablesSO == null) return;
            List<SceneVar> sceneVars = sceneVariablesSO.sceneVars;
            SetSceneVars(sceneVars);
        }
        public static void SetSceneVars(List<SceneVar> sceneVars)
        {
            foreach (SceneVar sceneVar in sceneVars)
                AddVar(sceneVar);
        }

        private static void ModifySceneVar(int varUniqueID, SceneVar newVar)
        {
            if (SceneVariables.ContainsKey(varUniqueID))
            {
                SceneVariables[varUniqueID] = newVar;
                ChangedVar(varUniqueID);
            }
            IncorrectID(varUniqueID);
        }
        public static void ModifyBoolVar(int varUniqueID, BoolOperation op, bool param = false)
        {
            if (SceneVariables.ContainsKey(varUniqueID))
            {
                SceneVar var = SceneVariables[varUniqueID];
                if (var.type == SceneVarType.BOOL && !var.isStatic)
                {
                    switch (op)
                    {
                        case BoolOperation.SET:
                            SceneVariables[varUniqueID].boolValue = param;
                            break;
                        case BoolOperation.INVERSE:
                            SceneVariables[varUniqueID].boolValue = !SceneVariables[varUniqueID].boolValue;
                            break;

                        default:
                            SceneVariables[varUniqueID].boolValue = param;
                            break;
                    }
                    ChangedVar(varUniqueID);
                    return;
                }
                IncorrectType(varUniqueID, SceneVarType.BOOL);
                return;
            }
            IncorrectID(varUniqueID);
        }
        public static void ModifyIntVar(int varUniqueID, IntOperation op, int param)
        {
            if (SceneVariables.ContainsKey(varUniqueID))
            {
                SceneVar var = SceneVariables[varUniqueID];
                if (var.type == SceneVarType.INT && !var.isStatic)
                {
                    switch (op)
                    {
                        case IntOperation.SET:
                            SceneVariables[varUniqueID].intValue = param;
                            break;
                        case IntOperation.ADD:
                            SceneVariables[varUniqueID].intValue += param;
                            break;

                        default:
                            SceneVariables[varUniqueID].intValue = param;
                            break;
                    }
                    ChangedVar(varUniqueID);
                    return;
                }
                IncorrectType(varUniqueID, SceneVarType.INT);
                return;
            }
            IncorrectID(varUniqueID);
        }
        public static void ModifyFloatVar(int varUniqueID, FloatOperation op, float param)
        {
            if (SceneVariables.ContainsKey(varUniqueID))
            {
                SceneVar var = SceneVariables[varUniqueID];
                if (var.type == SceneVarType.FLOAT && !var.isStatic)
                {
                    switch (op)
                    {
                        case FloatOperation.SET:
                            SceneVariables[varUniqueID].floatValue = param;
                            break;
                        case FloatOperation.ADD:
                            SceneVariables[varUniqueID].floatValue += param;
                            break;

                        default:
                            SceneVariables[varUniqueID].floatValue = param;
                            break;
                    }
                    ChangedVar(varUniqueID);
                    return;
                }
                IncorrectType(varUniqueID, SceneVarType.FLOAT);
                return;
            }
            IncorrectID(varUniqueID);
        }
        public static void ModifyStringVar(int varUniqueID, StringOperation op, string param)
        {
            if (SceneVariables.ContainsKey(varUniqueID))
            {
                SceneVar var = SceneVariables[varUniqueID];
                if (var.type == SceneVarType.STRING && !var.isStatic)
                {
                    switch (op)
                    {
                        case StringOperation.SET:
                            SceneVariables[varUniqueID].stringValue = param;
                            break;
                        case StringOperation.APPEND:
                            SceneVariables[varUniqueID].stringValue += param;
                            break;
                        case StringOperation.REMOVE:
                            SceneVariables[varUniqueID].stringValue.Replace(param, "");
                            break;

                        default:
                            SceneVariables[varUniqueID].stringValue = param;
                            break;
                    }
                    ChangedVar(varUniqueID);
                    return;
                }
                IncorrectType(varUniqueID, SceneVarType.FLOAT);
                return;
            }
            IncorrectID(varUniqueID);
        }
        #endregion

        #region Log
        private static void IncorrectID(int ID)
        {
            Debug.LogError("Variable ID : '" + ID + "' doesn't exist in the current scene.");
        }
        private static void IncorrectType(int ID, SceneVarType type)
        {
            Debug.LogError("Variable ID : '" + ID + "' is not of type : '" + type.ToString() + "'.");
        }
        #endregion
    }
}
