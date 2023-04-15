using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Dhs5.Utility.SceneCreation
{
    [Serializable]
    public class SceneCondition
    {
        public SceneVariablesSO sceneVariablesSO;

        public int var1UniqueID;
        public int var2UniqueID;

        public SceneVar SceneVar1 { get => SceneState.GetSceneVar(var1UniqueID); }
        public SceneVar SceneVar2 { get => SceneState.GetSceneVar(var2UniqueID); }

        public BoolComparison boolComp;

        public IntComparison intComp;

        public FloatComparison floatComp;

        public StringComparison stringComp;


        public bool VerifyCondition()
        {
            switch (SceneVar1.type)
            {
                case SceneVarType.BOOL:
                    if (SceneVar2.type != SceneVarType.BOOL) return VerifyBoolCondition(SceneVar1.boolValue, CastToBool(SceneVar2));
                    return VerifyBoolCondition(SceneVar1.boolValue, SceneVar2.boolValue);
                case SceneVarType.INT:
                    if (SceneVar2.type == SceneVarType.BOOL || SceneVar2.type == SceneVarType.STRING) return VerifyIntCondition(SceneVar1.intValue, CastToInt(SceneVar2));
                    if (SceneVar2.type == SceneVarType.INT) return VerifyIntCondition(SceneVar1.intValue, SceneVar2.intValue);
                    return VerifyIntCondition(SceneVar1.intValue, SceneVar2.floatValue);
                case SceneVarType.FLOAT:
                    if (SceneVar2.type == SceneVarType.BOOL || SceneVar2.type == SceneVarType.STRING) return VerifyFloatCondition(SceneVar1.intValue, CastToFloat(SceneVar2));
                    if (SceneVar2.type == SceneVarType.INT) return VerifyFloatCondition(SceneVar1.intValue, SceneVar2.intValue);
                    return VerifyFloatCondition(SceneVar1.intValue, SceneVar2.floatValue);
                case SceneVarType.STRING:
                    if (SceneVar2.type != SceneVarType.STRING) return VerifyStringCondition(SceneVar1.stringValue, CastToString(SceneVar2));
                    return VerifyStringCondition(SceneVar1.stringValue, SceneVar2.stringValue);
            }

            return true;
        }

        #region Verify with Type
        private bool VerifyBoolCondition(bool valueToCompare, bool valueToCompareTo)
        {
            switch (boolComp)
            {
                case BoolComparison.EQUAL:
                    return valueToCompare == valueToCompareTo;
                case BoolComparison.DIFF:
                    return valueToCompare != valueToCompareTo;
            }
            return true;
        }
        private bool VerifyIntCondition(int valueToCompare, int valueToCompareTo)
        {
            switch (intComp)
            {
                case IntComparison.EQUAL:
                    return valueToCompare == valueToCompareTo;
                case IntComparison.DIFF:
                    return valueToCompare != valueToCompareTo;
                case IntComparison.SUP:
                    return valueToCompare > valueToCompareTo;
                case IntComparison.SUP_EQUAL:
                    return valueToCompare >= valueToCompareTo;
                case IntComparison.INF:
                    return valueToCompare < valueToCompareTo;
                case IntComparison.INF_EQUAL:
                    return valueToCompare <= valueToCompareTo;
            }
            return true;
        }
        private bool VerifyIntCondition(int valueToCompare, float valueToCompareTo)
        {
            switch (intComp)
            {
                case IntComparison.EQUAL:
                    return valueToCompare == valueToCompareTo;
                case IntComparison.DIFF:
                    return valueToCompare != valueToCompareTo;
                case IntComparison.SUP:
                    return valueToCompare > valueToCompareTo;
                case IntComparison.SUP_EQUAL:
                    return valueToCompare >= valueToCompareTo;
                case IntComparison.INF:
                    return valueToCompare < valueToCompareTo;
                case IntComparison.INF_EQUAL:
                    return valueToCompare <= valueToCompareTo;
            }
            return true;
        }
        private bool VerifyFloatCondition(float valueToCompare, float valueToCompareTo)
        {
            switch (floatComp)
            {
                case FloatComparison.EQUAL:
                    return valueToCompare == valueToCompareTo;
                case FloatComparison.DIFF:
                    return valueToCompare != valueToCompareTo;
                case FloatComparison.SUP:
                    return valueToCompare > valueToCompareTo;
                case FloatComparison.SUP_EQUAL:
                    return valueToCompare >= valueToCompareTo;
                case FloatComparison.INF:
                    return valueToCompare < valueToCompareTo;
                case FloatComparison.INF_EQUAL:
                    return valueToCompare <= valueToCompareTo;
            }
            return true;
        }
        private bool VerifyFloatCondition(float valueToCompare, int valueToCompareTo)
        {
            switch (floatComp)
            {
                case FloatComparison.EQUAL:
                    return valueToCompare == valueToCompareTo;
                case FloatComparison.DIFF:
                    return valueToCompare != valueToCompareTo;
                case FloatComparison.SUP:
                    return valueToCompare > valueToCompareTo;
                case FloatComparison.SUP_EQUAL:
                    return valueToCompare >= valueToCompareTo;
                case FloatComparison.INF:
                    return valueToCompare < valueToCompareTo;
                case FloatComparison.INF_EQUAL:
                    return valueToCompare <= valueToCompareTo;
            }
            return true;
        }
        private bool VerifyStringCondition(string valueToCompare, string valueToCompareTo)
        {
            switch (stringComp)
            {
                case StringComparison.EQUAL:
                    return valueToCompare == valueToCompareTo;
                case StringComparison.DIFF:
                    return valueToCompare != valueToCompareTo;
                case StringComparison.CONTAINS:
                    return valueToCompare.Contains(valueToCompareTo);
                case StringComparison.CONTAINED:
                    return valueToCompareTo.Contains(valueToCompare);
                case StringComparison.NULL_EMPTY:
                    return String.IsNullOrEmpty(valueToCompare);
            }
            return true;
        }
        #endregion

        #region Cast To Bool
        private bool CastToBool(SceneVar var)
        {
            switch (var.type)
            {
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
        private int CastToInt(SceneVar var)
        {
            int i;
            switch (var.type)
            {
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
        private float CastToFloat(SceneVar var)
        {
            float f;
            switch (var.type)
            {
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
        private string CastToString(SceneVar var)
        {
            switch (var.type)
            {
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
    }
}
