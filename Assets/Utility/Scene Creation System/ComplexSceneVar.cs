using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dhs5.Utility.SceneCreation
{
    [System.Serializable]
    public enum ComplexSceneVarType
    {
        CONDITION = 0,
        TOTAL_INT = 1,
        TOTAL_FLOAT = 2,
        SENTENCE = 3,
    }

    [System.Serializable]
    public class ComplexSceneVar : SceneState.ISceneVarSetupable
    {
        #region SetUp
        public void SetUp(SceneVariablesSO sceneVariablesSO)
        {
            conditions.SetUp(sceneVariablesSO);

            UpdateLinkInfo();
        }
        private void UpdateLinkInfo()
        {
            if (Link != null)
            {
                Link.uniqueID = uniqueID;
                Link.ID = ID;
                Link.type = BaseType;
            }
        }
        #endregion

        public ComplexSceneVar(ComplexSceneVar var, SceneVar _link)
        {
            uniqueID = var.uniqueID;
            ID = var.ID;
            type = var.type;

            conditions = var.conditions;
            Link = _link;
        }

        public int uniqueID = 0;

        public string ID;
        public ComplexSceneVarType type;
        public SceneVarType BaseType
        {
            get
            {
                switch (type)
                {
                    case ComplexSceneVarType.CONDITION: return SceneVarType.BOOL;
                    case ComplexSceneVarType.TOTAL_INT: return SceneVarType.INT;
                    case ComplexSceneVarType.TOTAL_FLOAT: return SceneVarType.FLOAT;
                    case ComplexSceneVarType.SENTENCE: return SceneVarType.STRING;
                    default: return SceneVarType.BOOL;
                }
            }
        }

        public List<SceneCondition> conditions;
        // TotalInt
        // TotalFloat
        // Sentence

        public object Value
        {
            get
            {
                switch (type)
                {
                    case ComplexSceneVarType.CONDITION: return conditions.VerifyConditions();
                    case ComplexSceneVarType.TOTAL_INT: return 0;
                    case ComplexSceneVarType.TOTAL_FLOAT: return 0f;
                    case ComplexSceneVarType.SENTENCE: return "";
                    default: return null;
                }
            }
        }
        public List<int> Dependencies
        {
            get
            {
                return new();
            }
        }


        public int Reset
        {
            set
            {
                if (value != 911) return;

                uniqueID = 0;
                ID = "";
                type = ComplexSceneVarType.CONDITION;

                conditions = new();
            }
        }

        [NonSerialized] public SceneVar Link;

        [SerializeField] private float propertyHeight;
    }
}
