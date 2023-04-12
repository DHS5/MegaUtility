using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Dhs5.Utility.SceneCreation
{
    [Serializable]
    public class SceneVar
    {
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
    }
}
