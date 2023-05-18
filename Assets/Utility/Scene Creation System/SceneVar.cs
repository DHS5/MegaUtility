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
            isLink = var.isLink;
        }
        private SceneVar(int UID, string id, SceneVarType _type, bool _isStatic, bool _isLink)
        {
            uniqueID = UID;
            ID = id;
            type = _type;
            isStatic = _isStatic;
            isLink = _isLink;

            switch (type)
            {
                case SceneVarType.BOOL: 
                    boolValue = false; 
                    break;
                case SceneVarType.INT: 
                    intValue = 0;
                    break;
                case SceneVarType.FLOAT: 
                    floatValue = 0f;
                    break;
                case SceneVarType.STRING: 
                    stringValue = "";
                    break;
            }
        }
        public static SceneVar CreateLink(ComplexSceneVar var)
        {
            return new(var.uniqueID, var.ID, var.BaseType, false, true);
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

        [SerializeField] private bool boolValue;
        [SerializeField] private int intValue;
        [SerializeField] private float floatValue;
        [SerializeField] private string stringValue;

        public bool hasMin;
        public bool hasMax;
        public int minInt;
        public int maxInt;
        public float minFloat;
        public float maxFloat;

        [SerializeField] private float propertyHeight;

        #region Values
        public bool BoolValue
        {
            get
            {
                if (isLink) return (bool)LinkValue;
                return boolValue;
            }
            set
            {
                if (isLink)
                {
                    CantSetLinkVar();
                    return;
                }
                boolValue = value;
            }
        }
        public int IntValue
        {
            get
            {
                if (isLink)
                {
                    return (int)LinkValue;
                }
                return intValue;
            }
            set
            {
                if (isLink)
                {
                    CantSetLinkVar();
                    return;
                }
                intValue = value;
            }
        }
        public float FloatValue
        {
            get
            {
                if (isLink) return (float)LinkValue;
                return floatValue;
            }
            set
            {
                if (isLink)
                {
                    CantSetLinkVar();
                    return;
                }
                floatValue = value;
            }
        }
        public string StringValue
        {
            get
            {
                if (isLink) return (string)LinkValue;
                return stringValue;
            }
            set
            {
                if (isLink)
                {
                    CantSetLinkVar();
                    return;
                }
                stringValue = value;
            }
        }
        public object Value
        {
            get
            {
                if (IsLink) return LinkValue;

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
        public object LinkValue => SceneState.GetComplexSceneVarValue(uniqueID);
        #endregion

        public override string ToString()
        {
            if (type == SceneVarType.EVENT) return ID + " (EVENT)";
            if (IsLink) return ID + " (" + type.ToString() + " LINK)";
            return ID + " (" + type.ToString() + ") = " + Value + (isStatic ? " (static)" : "");
        }
        public string PopupString()
        {
            if (type == SceneVarType.EVENT) return ID + " (EVENT)";
            if (IsLink) return ID + " (" + type.ToString() + " LINK)";
            return ID + " (" + type.ToString() + ")" + (isStatic ? " = " + Value : "");
        }

        [SerializeField] private bool isStatic = false;
        public bool IsStatic => isStatic;

        [SerializeField] private bool isLink = false;
        public bool IsLink => isLink;


        private void CantSetLinkVar()
        {
            Debug.LogError("This SceneVar is a link to a ComplexSceneVar, you can't set its value");
        }
    }
}
