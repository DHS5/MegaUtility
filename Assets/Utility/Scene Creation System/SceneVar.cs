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

                isStatic = false;
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
                    case SceneVarType.EVENT: return null;
                }
                return null;
            }
        }

        public override string ToString()
        {
            if (type == SceneVarType.EVENT) return ID + " (EVENT)";
            return ID + " (" + type.ToString() + ") = " + Value + (isStatic ? " (static)" : "");
        }
        public string PopupString()
        {
            if (type == SceneVarType.EVENT) return ID + " (EVENT)";
            return ID + " (" + type.ToString() + ")" + (isStatic ? " = " + Value : "");
        }

        public bool isStatic = false;
    }
}
