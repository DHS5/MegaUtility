using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Dhs5.Utility.SceneCreation
{
    [Serializable]
    public class SceneVarTween
    {
        [Serializable]
        public enum BoolType
        {
            VAR = 0,
            CONDITION = 1,
        }

        [SerializeField] private SceneVarType type;

        [SerializeField] private SceneVariablesSO sceneVariablesSO;

        [SerializeField] private int sceneVarUniqueID;

        // Static
        [SerializeField] private bool canBeStatic;
        [SerializeField] private bool isStatic;

        [SerializeField] private bool boolValue;
        [SerializeField] private int intValue;
        [SerializeField] private float floatValue;
        [SerializeField] private string stringValue;

        [SerializeField] private BoolType boolType;
        [SerializeField] private List<SceneCondition> sceneConditions;

        [SerializeField] private float propertyHeight;

        private SceneVar SceneVar
        {
            get => SceneState.GetSceneVar(sceneVarUniqueID);
        }


        public void SetUp(SceneVariablesSO _sceneVariablesSO, SceneVarType _type, bool _canBeStatic = false)
        {
            sceneVariablesSO = _sceneVariablesSO;
            type = _type;
            if (type != SceneVarType.EVENT) canBeStatic = _canBeStatic;
            if (type == SceneVarType.BOOL) sceneConditions.SetUp(sceneVariablesSO);
        }

        private bool IsStatic => canBeStatic && isStatic;
        public object Value
        {
            get
            {
                switch (type)
                {
                    case SceneVarType.BOOL: return BoolValue;
                    case SceneVarType.INT: return IntValue;
                    case SceneVarType.FLOAT: return FloatValue;
                    case SceneVarType.STRING: return StringValue;
                    default: return null;
                }
            }
        }
        public bool BoolValue
        {
            get
            {
                if (IsStatic) return boolValue;
                if (boolType == BoolType.CONDITION) return sceneConditions.VerifyConditions();
                if (SceneVar.type != SceneVarType.BOOL) IncorrectType(SceneVarType.BOOL);
                return SceneVar.boolValue;
            }
            set
            {
                if (boolType == BoolType.CONDITION)
                {
                    CantSetCondition();
                    return;
                }
                if (SceneVar.type != SceneVarType.BOOL)
                {
                    IncorrectType(SceneVarType.BOOL);
                    return;
                }
                if (IsStatic)
                {
                    boolValue = value;
                    return;
                }
                SceneState.ModifyBoolVar(sceneVarUniqueID, BoolOperation.SET, value);
            }
        }
        public int IntValue
        {
            get
            {
                if (IsStatic) return intValue;
                if (SceneVar.type == SceneVarType.FLOAT) return (int)SceneVar.floatValue;
                if (SceneVar.type != SceneVarType.INT) IncorrectType(SceneVarType.INT);
                return SceneVar.intValue;
            }
            set
            {
                if (SceneVar.type != SceneVarType.INT)
                {
                    IncorrectType(SceneVarType.INT);
                    return;
                }
                if (IsStatic)
                {
                    intValue = value;
                    return;
                }
                SceneState.ModifyIntVar(sceneVarUniqueID, IntOperation.SET, value);
            }
        }
        public float FloatValue
        {
            get
            {
                if (IsStatic) return floatValue;
                if (SceneVar.type == SceneVarType.INT) return SceneVar.intValue;
                if (SceneVar.type != SceneVarType.FLOAT) IncorrectType(SceneVarType.FLOAT);
                return SceneVar.floatValue;
            }
            set
            {
                if (SceneVar.type != SceneVarType.FLOAT)
                {
                    IncorrectType(SceneVarType.FLOAT);
                    return;
                }
                if (IsStatic)
                {
                    floatValue = value;
                    return;
                }
                SceneState.ModifyFloatVar(sceneVarUniqueID, FloatOperation.SET, value);
            }
        }
        public string StringValue
        {
            get
            {
                if (IsStatic) return stringValue;
                if (SceneVar.type != SceneVarType.STRING) IncorrectType(SceneVarType.STRING);
                return SceneVar.stringValue;
            }
            set
            {
                if (SceneVar.type != SceneVarType.STRING)
                {
                    IncorrectType(SceneVarType.STRING);
                    return;
                }
                if (IsStatic)
                {
                    stringValue = value;
                    return;
                }
                SceneState.ModifyStringVar(sceneVarUniqueID, StringOperation.SET, value);
            }
        }
        public void Trigger()
        {
            if (SceneVar.type != SceneVarType.EVENT)
            {
                IncorrectType(SceneVarType.EVENT);
                return;
            }
            SceneState.TriggerEventVar(sceneVarUniqueID);
        }


        private void IncorrectType(SceneVarType type)
        {
            ZDebug.LogE("This SceneVarTween is a ", SceneVar.type, " and not a ", type);
        }
        private void CantSetCondition()
        {
            ZDebug.LogE("This SceneVarTween is a SCENE CONDITION and can't be set");
        }
    }
}
