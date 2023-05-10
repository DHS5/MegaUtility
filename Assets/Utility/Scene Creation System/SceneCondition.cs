using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Dhs5.Utility.SceneCreation
{
    [Serializable]
    public class SceneCondition : SceneState.ISceneVarSetupable
    {
        public SceneVariablesSO sceneVariablesSO;

        [SerializeField] private int var1UniqueID;
        [SerializeField] private int var2UniqueID;

        public SceneVar SceneVar1 { get => SceneState.GetSceneVar(var1UniqueID); }
        public SceneVar SceneVar2 { get => SceneState.GetSceneVar(var2UniqueID); }

        public BoolComparison boolComp;

        public IntComparison intComp;

        public FloatComparison floatComp;

        public StringComparison stringComp;


        public LogicOperator logicOperator;

        public void SetUp(SceneVariablesSO sceneVariablesSO)
        {
            this.sceneVariablesSO = sceneVariablesSO;
        }
        
        public bool VerifyCondition()
        {
            switch (SceneVar1.type)
            {
                case SceneVarType.BOOL:
                    if (SceneVar2.type != SceneVarType.BOOL) return VerifyBoolCondition(SceneVar1.boolValue, SceneState.CastToBool(SceneVar2));
                    return VerifyBoolCondition(SceneVar1.boolValue, SceneVar2.boolValue);
                case SceneVarType.INT:
                    if (SceneVar2.type == SceneVarType.BOOL || SceneVar2.type == SceneVarType.STRING) return VerifyIntCondition(SceneVar1.intValue, SceneState.CastToInt(SceneVar2));
                    if (SceneVar2.type == SceneVarType.INT) return VerifyIntCondition(SceneVar1.intValue, SceneVar2.intValue);
                    return VerifyIntCondition(SceneVar1.intValue, SceneVar2.floatValue);
                case SceneVarType.FLOAT:
                    if (SceneVar2.type == SceneVarType.BOOL || SceneVar2.type == SceneVarType.STRING) return VerifyFloatCondition(SceneVar1.intValue, SceneState.CastToFloat(SceneVar2));
                    if (SceneVar2.type == SceneVarType.INT) return VerifyFloatCondition(SceneVar1.intValue, SceneVar2.intValue);
                    return VerifyFloatCondition(SceneVar1.intValue, SceneVar2.floatValue);
                case SceneVarType.STRING:
                    if (SceneVar2.type != SceneVarType.STRING) return VerifyStringCondition(SceneVar1.stringValue, SceneState.CastToString(SceneVar2));
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

        #region Operator Description
        public static string BoolCompDescription(BoolComparison comp)
        {
            switch (comp)
            {
                case BoolComparison.EQUAL: return " == ";
                case BoolComparison.DIFF: return " != ";
                default: return "";
            }
        }
        public static string IntCompDescription(IntComparison comp)
        {
            switch (comp)
            {
                case IntComparison.EQUAL: return " == ";
                case IntComparison.DIFF: return " != ";
                case IntComparison.SUP: return " > ";
                case IntComparison.INF: return " < ";
                case IntComparison.SUP_EQUAL: return " >= ";
                case IntComparison.INF_EQUAL: return " <= ";
                default: return "";
            }
        }
        public static string FloatCompDescription(FloatComparison comp)
        {
            switch (comp)
            {
                case FloatComparison.EQUAL: return " == ";
                case FloatComparison.DIFF: return " != ";
                case FloatComparison.SUP: return " > ";
                case FloatComparison.INF: return " < ";
                case FloatComparison.SUP_EQUAL: return " >= ";
                case FloatComparison.INF_EQUAL: return " <= ";
                default: return "";
            }
        }
        public static string StringCompDescription(StringComparison comp)
        {
            switch (comp)
            {
                case StringComparison.EQUAL: return " == ";
                case StringComparison.DIFF: return " != ";
                case StringComparison.CONTAINS: return " Contains : ";
                case StringComparison.CONTAINED: return " Contained in : ";
                case StringComparison.NULL_EMPTY: return " is null or empty. ";
                default: return "";
            }
        }
        #endregion
    }
}
