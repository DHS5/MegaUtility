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
    public enum IntOperation
    {
        SET, ADD
    }
    [Serializable]
    public enum FloatOperation
    {
        SET, ADD
    }
    [Serializable]
    public enum StringOperation
    {
        SET, APPEND, REMOVE
    }
    #endregion

    [Serializable]
    public class SceneVar
    {
        public string ID;
        public SceneVarType type;
        public object value;
    }

    public static class SceneState
    {
        private static Dictionary<string, SceneVar> SceneVariables = new();

        #region Private Utility functions
        private static void Clear()
        {
            SceneVariables.Clear();
        }
        private static void AddVar(SceneVar variable)
        {
            SceneVariables[variable.ID] = variable;
        }
        private static void ChangedVar(string varID)
        {
            if (SceneVariables.ContainsKey(varID))
            {
                SceneEventManager.TriggerEvent(varID, SceneVariables[varID]);
            }
        }
        #endregion

        #region Public accessors
        public static object GetObjectValue(string varID)
        {
            if (SceneVariables.ContainsKey(varID))
                return SceneVariables[varID].value;
            IncorrectID(varID);
            return null;
        }
        public static bool TryGetBoolValue(string varID, out bool value)
        {
            value = false;
            if (SceneVariables.ContainsKey(varID))
            {
                SceneVar sceneVar = SceneVariables[varID];
                if (sceneVar.type == SceneVarType.BOOL)
                {
                    value = (bool) sceneVar.value;
                    return true;
                }
                IncorrectType(varID, SceneVarType.BOOL);
                return false;
            }
            IncorrectID(varID);
            return false;
        }
        public static bool TryGetIntValue(string varID, out int value)
        {
            value = 0;
            if (SceneVariables.ContainsKey(varID))
            {
                SceneVar sceneVar = SceneVariables[varID];
                if (sceneVar.type == SceneVarType.INT)
                {
                    value = (int) sceneVar.value;
                    return true;
                }
                IncorrectType(varID, SceneVarType.INT);
                return false;
            }
            IncorrectID(varID);
            return false;
        }
        public static bool TryGetFloatValue(string varID, out float value)
        {
            value = 0f;
            if (SceneVariables.ContainsKey(varID))
            {
                SceneVar sceneVar = SceneVariables[varID];
                if (sceneVar.type == SceneVarType.FLOAT)
                {
                    value = (float) sceneVar.value;
                    return true;
                }
                IncorrectType(varID, SceneVarType.FLOAT);
                return false;
            }
            IncorrectID(varID);
            return false;
        }
        public static bool TryGetStringValue(string varID, out string value)
        {
            value = null;
            if (SceneVariables.ContainsKey(varID))
            {
                SceneVar sceneVar = SceneVariables[varID];
                if (sceneVar.type == SceneVarType.STRING)
                {
                    value = (string) sceneVar.value;
                    return true;
                }
                IncorrectType(varID, SceneVarType.STRING);
                return false;
            }
            IncorrectID(varID);
            return false;
        }
        #endregion

        #region Public setters
        public static void SetSceneVars(SceneVariablesSO sceneVariablesSO)
        {
            if (sceneVariablesSO == null) return;
            List<SceneVar> sceneVars = sceneVariablesSO.sceneVars;
            SetSceneVars(sceneVars);
        }
        public static void SetSceneVars(List<SceneVar> sceneVars)
        {
            foreach (SceneVar sceneVar in sceneVars)
                AddVar(sceneVar);
        }

        public static void ModifySceneVar(string varID, SceneVar newVar)
        {
            if (SceneVariables.ContainsKey(varID))
            {
                SceneVariables[varID] = newVar;
                ChangedVar(varID);
            }
            IncorrectID(varID);
        }
        public static void ModifyBoolVar(string varID, BoolOperation op, bool param = false)
        {
            if (SceneVariables.ContainsKey(varID))
            {
                SceneVar var = SceneVariables[varID];
                if (var.type == SceneVarType.BOOL)
                {
                    switch (op)
                    {
                        case BoolOperation.SET:
                            SceneVariables[varID].value = param;
                            break;
                        case BoolOperation.INVERSE:
                            SceneVariables[varID].value = !((bool)SceneVariables[varID].value);
                            break;

                        default:
                            SceneVariables[varID].value = param;
                            break;
                    }
                    ChangedVar(varID);
                    return;
                }
                IncorrectType(varID, SceneVarType.BOOL);
                return;
            }
            IncorrectID(varID);
        }
        public static void ModifyIntVar(string varID, IntOperation op, int param)
        {
            if (SceneVariables.ContainsKey(varID))
            {
                SceneVar var = SceneVariables[varID];
                if (var.type == SceneVarType.INT)
                {
                    switch (op)
                    {
                        case IntOperation.SET:
                            SceneVariables[varID].value = param;
                            break;
                        case IntOperation.ADD:
                            SceneVariables[varID].value = (int)SceneVariables[varID].value + param;
                            break;

                        default:
                            SceneVariables[varID].value = param;
                            break;
                    }
                    ChangedVar(varID);
                    return;
                }
                IncorrectType(varID, SceneVarType.INT);
                return;
            }
            IncorrectID(varID);
        }
        public static void ModifyFloatVar(string varID, FloatOperation op, float param)
        {
            if (SceneVariables.ContainsKey(varID))
            {
                SceneVar var = SceneVariables[varID];
                if (var.type == SceneVarType.FLOAT)
                {
                    switch (op)
                    {
                        case FloatOperation.SET:
                            SceneVariables[varID].value = param;
                            break;
                        case FloatOperation.ADD:
                            SceneVariables[varID].value = (float)SceneVariables[varID].value + param;
                            break;

                        default:
                            SceneVariables[varID].value = param;
                            break;
                    }
                    ChangedVar(varID);
                    return;
                }
                IncorrectType(varID, SceneVarType.FLOAT);
                return;
            }
            IncorrectID(varID);
        }
        public static void ModifyStringVar(string varID, StringOperation op, string param)
        {
            if (SceneVariables.ContainsKey(varID))
            {
                SceneVar var = SceneVariables[varID];
                if (var.type == SceneVarType.STRING)
                {
                    switch (op)
                    {
                        case StringOperation.SET:
                            SceneVariables[varID].value = param;
                            break;
                        case StringOperation.APPEND:
                            SceneVariables[varID].value = (string)SceneVariables[varID].value + param;
                            break;
                        case StringOperation.REMOVE:
                            SceneVariables[varID].value = ((string)SceneVariables[varID].value).Replace(param, "");
                            break;

                        default:
                            SceneVariables[varID].value = param;
                            break;
                    }
                    ChangedVar(varID);
                    return;
                }
                IncorrectType(varID, SceneVarType.FLOAT);
                return;
            }
            IncorrectID(varID);
        }
        #endregion

        #region Log
        private static void IncorrectID(string ID)
        {
            ZDebug.LogE("Variable ID : '", ID, "' doesn't exist in the current scene.");
        }
        private static void IncorrectType(string ID, SceneVarType type)
        {
            ZDebug.LogE("Variable ID : '", ID, "' is not of type : '", type.ToString(), "'.");
        }
        #endregion
    }
}
