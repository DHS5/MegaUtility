using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Dhs5.Utility.SceneCreation
{
    [Serializable]
    public class SceneAction
    {
        [HideInInspector] public SceneVariablesSO sceneVariablesSO;

        public string triggerID;

        private SceneVar sceneVar;
        public string varID;

        [Header("Bool")]
        public BoolOperation boolOP;
        public bool boolValue;

        [Header("Int")]
        public IntOperation intOP;
        public int intValue;

        [Header("Float")]
        public FloatOperation floatOP;
        public float floatValue;

        [Header("String")]
        public StringOperation stringOP;
        public string stringValue;

        /// <summary>
        /// Called everytime the list size is changed, for each SceneAction still existing AFTER the list size change
        /// </summary>
        public SceneAction()
        {
            Debug.Log("Init");
        }


        public void Trigger()
        {
            if (sceneVar == null) return;

            switch (sceneVar.type)
            {
                case SceneVarType.BOOL:
                    SceneState.ModifyBoolVar(sceneVar.ID, boolOP, boolValue);
                    break;
                case SceneVarType.INT:
                    SceneState.ModifyIntVar(sceneVar.ID, intOP, intValue);
                    break;
                case SceneVarType.FLOAT:
                    SceneState.ModifyFloatVar(sceneVar.ID, floatOP, floatValue);
                    break;
                case SceneVarType.STRING:
                    SceneState.ModifyStringVar(sceneVar.ID, stringOP, stringValue);
                    break;

                default:
                    break;
            }
        }
    }
}
