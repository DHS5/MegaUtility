using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Globalization;

namespace Dhs5.Utility.SceneCreation
{
    [Serializable]
    public class SceneVarTween
    {
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

        private SceneVar SceneVar
        {
            get => SceneState.GetSceneVar(sceneVarUniqueID);
        }


        public void SetUp(SceneVariablesSO _sceneVariablesSO, SceneVarType _type, bool _canBeStatic = false)
        {
            sceneVariablesSO = _sceneVariablesSO;
            type = _type;
            if (type != SceneVarType.EVENT) canBeStatic = _canBeStatic;
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
                if (SceneVar.type != SceneVarType.BOOL) IncorrectType(SceneVarType.BOOL);
                return IsStatic ? boolValue : SceneVar.boolValue;
            }
            set
            {
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
                if (SceneVar.type == SceneVarType.FLOAT) return IsStatic ? (int)floatValue : (int)SceneVar.floatValue;
                if (SceneVar.type != SceneVarType.INT) IncorrectType(SceneVarType.INT);
                return IsStatic ? intValue : SceneVar.intValue;
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
                if (SceneVar.type == SceneVarType.INT) return IsStatic ? intValue : SceneVar.intValue;
                if (SceneVar.type != SceneVarType.FLOAT) IncorrectType(SceneVarType.FLOAT);
                return IsStatic ? floatValue : SceneVar.floatValue;
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
                if (SceneVar.type != SceneVarType.STRING) IncorrectType(SceneVarType.STRING);
                return IsStatic ? stringValue : SceneVar.stringValue;
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
    }
}
