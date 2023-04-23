using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Dhs5.Utility.SceneCreation
{
    [Serializable]
    public class SceneAction : SceneState.ISceneVarSetupable
    {
        public SceneVariablesSO sceneVariablesSO;

        public int var1UniqueID;
        public int var2UniqueID;

        public SceneVar SceneVar1 { get => SceneState.GetSceneVar(var1UniqueID); }
        public SceneVar SceneVar2 { get => SceneState.GetSceneVar(var2UniqueID); }

        // Operations        
        public BoolOperation boolOP;
        
        public IntOperation intOP;
        
        public FloatOperation floatOP;
        
        public StringOperation stringOP;

        public void SetUp(SceneVariablesSO sceneVariablesSO)
        {
            this.sceneVariablesSO = sceneVariablesSO;
        }
        
        public void Trigger()
        {
            if (SceneVar1 == null)
            {
                Debug.LogError("Trigger doesn't have a SceneVar");
                return;
            }

            switch (SceneVar1.type)
            {
                case SceneVarType.BOOL:
                    SceneState.ModifyBoolVar(SceneVar1.uniqueID, boolOP, SceneState.CastToBool(SceneVar2));
                    break;
                case SceneVarType.INT:
                    SceneState.ModifyIntVar(SceneVar1.uniqueID, intOP, SceneState.CastToInt(SceneVar2));
                    break;
                case SceneVarType.FLOAT:
                    SceneState.ModifyFloatVar(SceneVar1.uniqueID, floatOP, SceneState.CastToFloat(SceneVar2));
                    break;
                case SceneVarType.STRING:
                    SceneState.ModifyStringVar(SceneVar1.uniqueID, stringOP, SceneState.CastToString(SceneVar2));
                    break;
                case SceneVarType.EVENT:
                    SceneState.TriggerEventVar(SceneVar1.uniqueID);
                    break;

                default:
                    break;
            }
        }

        #region Operation Description
        public static string BoolOpDescription(BoolOperation op)
        {
            switch (op)
            {
                case BoolOperation.SET: return " = ";
                case BoolOperation.INVERSE: return " Inverse.";
                default: return "";
            }
        }
        public static string IntOpDescription(IntOperation op)
        {
            switch (op)
            {
                case IntOperation.SET: return " = ";
                case IntOperation.ADD: return " += ";
                case IntOperation.SUBSTRACT: return " -= ";
                case IntOperation.MULTIPLY: return " *= ";
                case IntOperation.DIVIDE: return " /= ";
                case IntOperation.POWER: return " = power ";
                default: return "";
            }
        }
        public static string FloatOpDescription(FloatOperation op)
        {
            switch (op)
            {
                case FloatOperation.SET: return " = ";
                case FloatOperation.ADD: return " += ";
                case FloatOperation.SUBSTRACT: return " -= ";
                case FloatOperation.MULTIPLY: return " *= ";
                case FloatOperation.DIVIDE: return " /= ";
                case FloatOperation.POWER: return " = power ";
                default: return "";
            }
        }
        public static string StringOpDescription(StringOperation op)
        {
            switch (op)
            {
                case StringOperation.SET: return " = ";
                case StringOperation.APPEND: return " .Append ";
                case StringOperation.REMOVE: return " .Replace(param,'') ";
                default: return "";
            }
        }

        #endregion
    }
}
