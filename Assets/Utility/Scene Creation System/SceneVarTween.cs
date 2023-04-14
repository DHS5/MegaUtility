using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Dhs5.Utility.SceneCreation
{
    [Serializable]
    public class SceneVarTween
    {
        public SceneVariablesSO sceneVariablesSO;

        public int sceneVarUniqueID;

        private SceneVar SceneVar
        {
            get => SceneState.GetSceneVar(sceneVarUniqueID);
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
        }
        public int IntValue
        {
            get
            {
                if (SceneVar.type != SceneVarType.INT) IncorrectType(SceneVarType.INT);
                return SceneVar.intValue;
            }
        }
        public float FloatValue
        {
            get
            {
                if (SceneVar.type != SceneVarType.FLOAT) IncorrectType(SceneVarType.FLOAT);
                return SceneVar.floatValue;
            }
        }
        public string StringValue
        {
            get
            {
                if (SceneVar.type != SceneVarType.STRING) IncorrectType(SceneVarType.STRING);
                return SceneVar.stringValue;
            }
        }


        private void IncorrectType(SceneVarType type)
        {
            ZDebug.LogE("This SceneVarTween is a ", SceneVar.type, " and not a ", type);
        }
    }
}
