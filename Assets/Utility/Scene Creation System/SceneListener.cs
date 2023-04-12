using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace Dhs5.Utility.SceneCreation
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
                    return VerifyBoolCondition(conditionIndex, SceneVar.boolValue, boolValue);
                case SceneVarType.INT:
                    return VerifyIntCondition(conditionIndex, SceneVar.intValue, intValue);
                case SceneVarType.FLOAT:
                    return VerifyFloatCondition(conditionIndex, SceneVar.floatValue, floatValue);
                case SceneVarType.STRING:
                    return VerifyStringCondition(conditionIndex, SceneVar.stringValue, stringValue);
            }

            return true;
        }

        private bool VerifyBoolCondition(int conditionIndex, bool valueToCompare, bool valueToCompareTo)
        {
            BoolComparison comp = (BoolComparison)conditionIndex;

            switch (comp)
            {
                case BoolComparison.EQUAL:
                    return valueToCompare == valueToCompareTo;
                case BoolComparison.DIFF:
                    return valueToCompare != valueToCompareTo;
            }
            return true;
        }
        private bool VerifyIntCondition(int conditionIndex, int valueToCompare, int valueToCompareTo)
        {
            IntComparison comp = (IntComparison)conditionIndex;

            switch (comp)
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
        private bool VerifyFloatCondition(int conditionIndex, float valueToCompare, float valueToCompareTo)
        {
            FloatComparison comp = (FloatComparison)conditionIndex;

            switch (comp)
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
        private bool VerifyStringCondition(int conditionIndex, string valueToCompare, string valueToCompareTo)
        {
            StringComparison comp = (StringComparison)conditionIndex;

            switch (comp)
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
