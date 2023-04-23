using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dhs5.Utility.SceneCreation
{
    [CreateAssetMenu(fileName = "SceneVars", menuName = "Scene Creation/Scene Vars")]
    public class SceneVariablesSO : ScriptableObject
    {
        public List<SceneVar> sceneVars;

        private int listSize = 0;

        public SceneVar this[int uniqueID]
        {
            get => sceneVars.Find(v => v.uniqueID == uniqueID);
        }

        public int GetUniqueIDByIndex(int index)
        {
            if (index < 0 || index >= sceneVars.Count) return 0;
            return sceneVars[index].uniqueID;
        }
        public int GetIndexByUniqueID(int uniqueID)
        {
            if (uniqueID == 0) return -1;
            return sceneVars.FindIndex(v => v.uniqueID == uniqueID);
        }
        
        private List<int> UniqueIDs
        {
            get
            {
                List<int> list = new();
                foreach (var var in sceneVars)
                {
                    list.Add(var.uniqueID);
                }
                return list;
            }
        }
        public List<string> IDs
        {
            get
            {
                List<string> list = new();
                foreach (var var in sceneVars)
                {
                    if (var.uniqueID != 0)
                        list.Add(var.ID);
                    else
                        list.Add("No unique ID");
                }
                return list;
            }
        }
        public List<string> SceneVarStrings
        {
            get
            {
                List<string> list = new();
                foreach (var var in sceneVars)
                {
                    if (var.uniqueID != 0)
                        list.Add(var.PopupString());
                    else
                        list.Add("No unique ID");
                }
                return list;
            }
        }
        
        public int GenerateUniqueID()
        {
            int uniqueID;
            do
            {
                uniqueID = Random.Range(1, 10000);
            } while (UniqueIDs.Contains(uniqueID));
            
            return uniqueID;
        }

        private void OnValidate()
        {
            if (listSize == sceneVars.Count - 1 && listSize != 0)
            {
                sceneVars[listSize].Reset = 911;
            }

            listSize = sceneVars.Count;
        }

        #region Lists
        public List<string> VarStrings(List<SceneVar> vars)
        {
            List<string> list = new();
            foreach (var var in vars)
            {
                if (var.uniqueID != 0)
                    list.Add(var.PopupString());
                else
                    list.Add("No unique ID");
            }
            return list;
        }
        public int GetUniqueIDByIndex(List<SceneVar> vars, int index)
        {
            if (index < 0 || index >= vars.Count) return 0;
            return vars[index].uniqueID;
        }
        public int GetIndexByUniqueID(List<SceneVar> vars, int uniqueID)
        {
            if (uniqueID == 0) return -1;
            return vars.FindIndex(v => v.uniqueID == uniqueID);
        }
        public List<SceneVar> GetListByType(SceneVarType type, bool precisely = false)
        {
            switch (type)
            {
                case SceneVarType.BOOL: return Booleans;
                case SceneVarType.INT: return precisely ? Integers : Numbers;
                case SceneVarType.FLOAT: return precisely ? Floats : Numbers;
                case SceneVarType.STRING: return Strings;
                case SceneVarType.EVENT: return Events;
                default: return sceneVars;
            }
        }

        public List<SceneVar> Statics
        {
            get => sceneVars.FindAll(v => v.isStatic);
        }
        public List<SceneVar> NonStatics
        {
            get => sceneVars.FindAll(v => !v.isStatic);
        }
        public List<SceneVar> Conditionable
        {
            get => sceneVars.FindAll(v => !v.isStatic && v.type != SceneVarType.EVENT);
        }
        public List<SceneVar> Booleans
        {
            get => sceneVars.FindAll(v => v.type == SceneVarType.BOOL);
        }
        public List<SceneVar> Numbers
        {
            get => sceneVars.FindAll(v => v.type == SceneVarType.INT || v.type == SceneVarType.FLOAT);
        }
        public List<SceneVar> Integers
        {
            get => sceneVars.FindAll(v => v.type == SceneVarType.INT);
        }
        public List<SceneVar> Floats
        {
            get => sceneVars.FindAll(v => v.type == SceneVarType.FLOAT);
        }
        public List<SceneVar> Strings
        {
            get => sceneVars.FindAll(v => v.type == SceneVarType.STRING);
        }
        public List<SceneVar> Events
        {
            get => sceneVars.FindAll(v => v.type == SceneVarType.EVENT);
        }
        public List<SceneVar> NonEvents
        {
            get => sceneVars.FindAll(v => v.type != SceneVarType.EVENT);
        }
        #endregion
    }
}
