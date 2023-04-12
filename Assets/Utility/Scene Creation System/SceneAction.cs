using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Dhs5.Utility.SceneCreation
{
    [Serializable]
    public class SceneAction
    {
        public SceneVariablesSO sceneVariablesSO;

        // Action specifics
        public string actionID;

        // SceneVar selection
        public int varUniqueID;
        public SceneVar SceneVar { get { return sceneVariablesSO[varUniqueID]; } }

        // Operations
        public int operationIndex;
        
        public BoolOperation boolOP;
        public bool boolValue;
        
        public IntOperation intOP;
        public int intValue;
        
        public FloatOperation floatOP;
        public float floatValue;
        
        public StringOperation stringOP;
        public string stringValue;


        public void Trigger()
        {
            if (SceneVar == null) return;

            switch (SceneVar.type)
            {
                case SceneVarType.BOOL:
                    SceneState.ModifyBoolVar(SceneVar.ID, boolOP, boolValue);
                    break;
                case SceneVarType.INT:
                    SceneState.ModifyIntVar(SceneVar.ID, intOP, intValue);
                    break;
                case SceneVarType.FLOAT:
                    SceneState.ModifyFloatVar(SceneVar.ID, floatOP, floatValue);
                    break;
                case SceneVarType.STRING:
                    SceneState.ModifyStringVar(SceneVar.ID, stringOP, stringValue);
                    break;

                default:
                    break;
            }
        }
    }
}
