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
        [SerializeField] private SceneVarType type = SceneVarType.FLOAT;

        [SerializeField] private SceneVariablesSO sceneVariablesSO;

        [SerializeField] private int sceneVarUniqueID;

        private SceneVar SceneVar
        {
            get => SceneState.GetSceneVar(sceneVarUniqueID);
        }


        public void SetUp(SceneVariablesSO _sceneVariablesSO, SceneVarType _type)
        {
            sceneVariablesSO = _sceneVariablesSO;
            type = _type;
        }


        public object Value
        {
            get => SceneVar.Value;
        }
        public bool BoolValue
        {
            get
            {
                if (SceneVar.type != SceneVarType.BOOL) IncorrectType(SceneVarType.BOOL);
                return SceneVar.boolValue;
            }
            set
            {
                if (SceneVar.type != SceneVarType.BOOL)
                {
                    IncorrectType(SceneVarType.BOOL);
                    return;
                }
                SceneState.ModifyBoolVar(sceneVarUniqueID, BoolOperation.SET, value);
            }
        }
        public int IntValue
        {
            get
            {
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
                SceneState.ModifyIntVar(sceneVarUniqueID, IntOperation.SET, value);
            }
        }
        public float FloatValue
        {
            get
            {
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
                SceneState.ModifyFloatVar(sceneVarUniqueID, FloatOperation.SET, value);
            }
        }
        public string StringValue
        {
            get
            {
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
