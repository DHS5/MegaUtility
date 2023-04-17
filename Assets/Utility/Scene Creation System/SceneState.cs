using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Codice.Client.Commands;
using Random = UnityEngine.Random;

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
        SET, ADD, SUBSTRACT, MULTIPLY, DIVIDE, POWER
    }
    [Serializable]
    public enum IntComparison
    {
        EQUAL, DIFF, SUP, INF, SUP_EQUAL, INF_EQUAL
    }
    [Serializable]
    public enum FloatOperation
    {
        SET, ADD, SUBSTRACT, MULTIPLY, DIVIDE, POWER
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
    [Serializable]
    public enum LogicOperator
    {
        AND, OR, NAND, NOR, XOR, XNOR
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
            SceneVariables[variable.uniqueID] = new(variable);
            //ChangedVar(variable.uniqueID);
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

        public static SceneVar GetSceneVar(int uniqueID)
        {
            return SceneVariables[uniqueID];
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
            List<SceneVar> sceneVars = new(sceneVariablesSO.sceneVars);
            SetSceneVars(sceneVars);
        }
        public static void SetSceneVars(List<SceneVar> sceneVars)
        {
            foreach (SceneVar sceneVar in sceneVars)
                AddVar(sceneVar);
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
                        case IntOperation.SUBSTRACT:
                            SceneVariables[varUniqueID].intValue -= param;
                            break;
                        case IntOperation.MULTIPLY:
                            SceneVariables[varUniqueID].intValue *= param;
                            break;
                        case IntOperation.DIVIDE:
                            SceneVariables[varUniqueID].intValue /= param;
                            break;
                        case IntOperation.POWER:
                            SceneVariables[varUniqueID].intValue = (int)Mathf.Pow(SceneVariables[varUniqueID].intValue, param);
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
                        case FloatOperation.SUBSTRACT:
                            SceneVariables[varUniqueID].floatValue -= param;
                            break;
                        case FloatOperation.MULTIPLY:
                            SceneVariables[varUniqueID].floatValue *= param;
                            break;
                        case FloatOperation.DIVIDE:
                            SceneVariables[varUniqueID].floatValue /= param;
                            break;
                        case FloatOperation.POWER:
                            SceneVariables[varUniqueID].floatValue = Mathf.Pow(SceneVariables[varUniqueID].floatValue, param);
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

        #region Casts
        #region Cast To Bool
        public static bool CastToBool(SceneVar var)
        {
            switch (var.type)
            {
                case SceneVarType.BOOL:
                    return var.boolValue;
                case SceneVarType.INT:
                    return var.intValue != 0;
                case SceneVarType.FLOAT:
                    return var.floatValue != 0;
                case SceneVarType.STRING:
                    return (var.stringValue.ToLower() == "true");
                default:
                    return false;
            }
        }
        #endregion

        #region Cast To Int
        public static int CastToInt(SceneVar var)
        {
            int i;
            switch (var.type)
            {
                case SceneVarType.INT:
                    return var.intValue;
                case SceneVarType.FLOAT:
                    return (int)var.floatValue;
                case SceneVarType.BOOL:
                    return var.boolValue ? 1 : 0;
                case SceneVarType.STRING:
                    try { i = int.Parse(var.stringValue); }
                    catch { i = 0; }
                    return i;
                default:
                    return 0;
            }
        }
        #endregion

        #region Cast To Float
        public static float CastToFloat(SceneVar var)
        {
            float f;
            switch (var.type)
            {
                case SceneVarType.FLOAT:
                    return var.floatValue;
                case SceneVarType.INT:
                    return var.intValue;
                case SceneVarType.BOOL:
                    return var.boolValue ? 1f : 0f;
                case SceneVarType.STRING:
                    try { f = float.Parse(var.stringValue); }
                    catch { f = 0f; }
                    return f;
                default:
                    return 0f;
            }
        }
        #endregion

        #region Cast To String
        public static string CastToString(SceneVar var)
        {
            switch (var.type)
            {
                case SceneVarType.STRING:
                    return var.stringValue;
                case SceneVarType.BOOL:
                    return var.boolValue.ToString();
                case SceneVarType.INT:
                    return var.intValue.ToString();
                case SceneVarType.FLOAT:
                    return var.floatValue.ToString();
                default:
                    return "";
            }
        }
        #endregion
        #endregion

        #region Extension Methods
        #region Set Ups
        public interface ISceneVarSetupable
        {
            public void SetUp(SceneVariablesSO sceneVariablesSO);
        }
        
        public static void SetUp<T>(this List<T> setupables, SceneVariablesSO sceneVariablesSO) where T : ISceneVarSetupable
        {
            if (setupables == null) return;

            foreach (var setupable in setupables)
            {
                setupable.SetUp(sceneVariablesSO);
            }
        }
        #endregion
        
        #region Scene Condition list verification (Extension Method)
        public static bool VerifyConditions(this List<SceneCondition> conditions)
        {
            if (conditions == null) return true;
        
            bool result = conditions[0].VerifyCondition();
        
            for (int i = 1; i < conditions.Count; i++)
            {
                result = ApplyLogicOperator(result, conditions[i - 1].logicOperator, conditions[i].VerifyCondition());
            }
        
            return false;
        }
        private static bool ApplyLogicOperator(bool bool1, LogicOperator op, bool bool2)
        {
            switch (op)
            {
                case LogicOperator.AND: return bool1 & bool2;
                case LogicOperator.OR: return bool1 | bool2;
                case LogicOperator.NAND: return !(bool1 & bool2);
                case LogicOperator.NOR: return !(bool1 | bool2);
                case LogicOperator.XOR: return bool1 ^ bool2;
                case LogicOperator.XNOR: return !(bool1 ^ bool2);
                default: return true;
            }
        }
        #endregion

        #region Random Scene Event triggering (Extension Method)
        /// <summary>
        /// Triggers a random SceneEvent in the list
        /// </summary>
        /// <param name="sceneEvents"></param>
        /// <param name="filter">Trigger a random SceneEvent among ones which eventID contains filter</param>
        public static void TriggerRandom(this List<SceneEvent> sceneEvents, string filter = null)
        {
            List<SceneEvent> events = new();

            if (filter != null)
            {
                foreach (SceneEvent sceneEvent in sceneEvents)
                    if (sceneEvent.eventID.Contains(filter))
                        events.Add(sceneEvent);
            }
            else
            {
                events = new(sceneEvents);
            }

            events[UnityEngine.Random.Range(0, events.Count)].Trigger();
        }
        #endregion
        
        #region Trigger a list of SceneEvents (Extension Method)
        /// <summary>
        /// Trigger every SceneEvent which eventID == ID in the list
        /// (Trigger all if ID == null)
        /// </summary>
        /// <param name="sceneEvents"></param>
        /// <param name="ID">ID of the SceneEvents to trigger</param>
        public static void Trigger(this List<SceneEvent> sceneEvents, string ID = null)
        {
            if (sceneEvents == null) return;
            
            List<SceneEvent> events = new();

            if (ID != null)
            {
                events = sceneEvents.FindAll(e => e.eventID == ID);
            }
            else
            {
                events = new(sceneEvents);
            }

            foreach (var sceneEvent in events)
            {
                sceneEvent.Trigger();
            }
        }
        #endregion
        
        #region Trigger a list of SceneTimelineEvents (Extension Method)
        /// <summary>
        /// Trigger every SceneTimelineEvent in the list
        /// (Trigger all if ID == null)
        /// </summary>
        /// <param name="sceneTEvents"></param>
        /// <param name="timelineID">timelineID of the SceneTimelineEvents to trigger</param>
        public static void Trigger(this List<SceneTimelineEvent> sceneTEvents, string timelineID)
        {
            if (sceneTEvents == null) return;
            
            foreach (var sceneEvent in sceneTEvents)
            {
                sceneEvent.Trigger(timelineID);
            }
        }
        #endregion
        #endregion
    }
}
