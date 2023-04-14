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
        public SceneVar CurrentSceneVar
        {
            get { return SceneState.GetSceneVar(varUniqueID); }
        }

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


        public bool debug = false;
        public float propertyHeight;


        public void Trigger()
        {
            if (SceneVar == null)
            {
                Debug.LogError("Trigger doesn't have a SceneVar");
                return;
            }

            switch (SceneVar.type)
            {
                case SceneVarType.BOOL:
                    SceneState.ModifyBoolVar(CurrentSceneVar.uniqueID, boolOP, boolValue);
                    break;
                case SceneVarType.INT:
                    SceneState.ModifyIntVar(CurrentSceneVar.uniqueID, intOP, intValue);
                    break;
                case SceneVarType.FLOAT:
                    SceneState.ModifyFloatVar(CurrentSceneVar.uniqueID, floatOP, floatValue);
                    break;
                case SceneVarType.STRING:
                    SceneState.ModifyStringVar(CurrentSceneVar.uniqueID, stringOP, stringValue);
                    break;

                default:
                    break;
            }
        }
    }
}
