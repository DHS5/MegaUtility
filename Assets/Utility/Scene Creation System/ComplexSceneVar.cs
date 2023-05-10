using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dhs5.Utility.SceneCreation
{
    public enum ComplexSceneVarType
    {
        CONDITION = 0,
        TOTAL_INT = 1,
        TOTAL_FLOAT = 2,
        SENTENCE = 3,
    }

    [System.Serializable]
    public class ComplexSceneVar
    {
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
        public void UpdateLinkValue()
        {
            if (Link == null) return;
            switch (type)
            {
                case ComplexSceneVarType.CONDITION: 
                    Link.boolValue = conditions.VerifyConditions();
                    break;
                case ComplexSceneVarType.TOTAL_INT:
                    Link.intValue = 0;
                    break;
                case ComplexSceneVarType.TOTAL_FLOAT: 
                    Link.floatValue = 0f;
                    break;
                case ComplexSceneVarType.SENTENCE:
                    Link.stringValue = "";
                    break;
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
    }
}
