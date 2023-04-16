using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Dhs5.Utility.SceneCreation
{
    [Serializable]
    public class SceneVar
    {
        public SceneVar(SceneVar var)
        {
            uniqueID = var.uniqueID;
            ID = var.ID;
            type = var.type;
            boolValue = var.boolValue;
            intValue = var.intValue;
            floatValue = var.floatValue;
            stringValue = var.stringValue;
            isStatic = var.isStatic;
        }
        
        public int Reset
        {
            set
            {
                if (value != 911) return;

                uniqueID = 0;
                ID = "";
                type = SceneVarType.BOOL;

                boolValue = false;
                intValue = 0;
                floatValue = 0f;
                stringValue = "";
            }
        }

        public int uniqueID = 0;
        
        public string ID;
        public SceneVarType type;
        
        public bool boolValue;
        public int intValue;
        public float floatValue;
        public string stringValue;

        public object Value
        {
            get
            {
                switch (type)
                {
                    case SceneVarType.BOOL: return boolValue;
                    case SceneVarType.INT: return intValue;
                    case SceneVarType.FLOAT: return floatValue;
                    case SceneVarType.STRING: return stringValue;
                }
                return null;
            }
        }

        public override string ToString()
        {
            return ID + " (" + type.ToString() + ") = " + Value;
        }

        public bool isStatic = false;
    }
}
