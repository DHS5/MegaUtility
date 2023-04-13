using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace SceneCreation
{
    [Serializable]
    public class SceneListener
    {
        public SceneVariablesSO sceneVariablesSO;

        // SceneVar selection
        public int varUniqueID;
        public SceneVar SceneVar { get { return sceneVariablesSO[varUniqueID]; } }
        
        // Condition
        public bool hasCondition;
        public int conditionIndex;

        public BoolComparison boolComp;
        public bool boolValue;
        
        public IntComparison intComp;
        public int intValue;

        public FloatComparison floatComp;
        public float floatValue;

        public StringComparison stringComp;
        public string stringValue;
        
        public UnityEvent<SceneVar> events;


        public bool VerifyCondition()
        {
            if (!hasCondition) return true;

            switch (SceneVar.type)
            {
                case SceneVarType.BOOL:
                    return VerifyBoolCondition(SceneVar.boolValue, boolValue);
                case SceneVarType.INT:
                    return VerifyIntCondition(SceneVar.intValue, intValue);
                case SceneVarType.FLOAT:
                    return VerifyFloatCondition(SceneVar.floatValue, floatValue);
                case SceneVarType.STRING:
                    return VerifyStringCondition(SceneVar.stringValue, stringValue);
            }

            return true;
        }

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
    }
}
