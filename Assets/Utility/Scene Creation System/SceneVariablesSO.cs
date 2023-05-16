using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Dhs5.Utility.SceneCreation
{
    [CreateAssetMenu(fileName = "SceneVars", menuName = "Scene Creation/Scene Vars")]
    public class SceneVariablesSO : ScriptableObject
    {
        public List<SceneVar> sceneVars;
        [HideInInspector]
        public List<SceneVar> sceneVarLinks;
        public List<ComplexSceneVar> complexSceneVars;

        private int listSize = 0;
        private int complexListSize = 0;

        public List<SceneVar> SceneVars => sceneVarLinks != null ? sceneVars.Concat(sceneVarLinks).ToList() : sceneVars;

        public SceneVar this[int uniqueID]
        {
            get => SceneVars.Find(v => v.uniqueID == uniqueID);
        }

        public int GetUniqueIDByIndex(int index)
        {
            if (index < 0 || index >= SceneVars.Count) return 0;
            return SceneVars[index].uniqueID;
        }
        public int GetIndexByUniqueID(int uniqueID)
        {
            if (uniqueID == 0) return -1;
            return SceneVars.FindIndex(v => v.uniqueID == uniqueID);
        }
        
        private List<int> UniqueIDs
        {
            get
            {
                List<int> list = new();
                foreach (var var in SceneVars)
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
                foreach (var var in SceneVars)
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
                foreach (var var in SceneVars)
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
            List<int> uniqueIDs = UniqueIDs;
            do
            {
                uniqueID = Random.Range(1, 10000);
            } while (uniqueIDs.Contains(uniqueID));
            
            return uniqueID;
        }

        private void OnValidate()
        {
            // Simple scene vars
            if (listSize == sceneVars.Count - 1 && listSize != 0)
            {
                sceneVars[listSize].Reset = 911;
            }

            listSize = sceneVars.Count;

            // Complex Scene vars
            if (complexListSize == complexSceneVars.Count - 1 && complexListSize != 0)
            {
                complexSceneVars[complexListSize].Reset = 911;
            }

            complexListSize = complexSceneVars.Count;

            ActuSceneVarLinks();

            complexSceneVars.SetUp(this);
            complexSceneVars.SetForbiddenUID(0);
        }

        private void ActuSceneVarLinks()
        {
            sceneVarLinks ??= new();
            if (complexSceneVars.Count > sceneVarLinks.Count)
            {
                foreach (var var in complexSceneVars)
                {
                    if (var.Link == null || var.Link.uniqueID == 0)
                    {
                        var.Link = SceneVar.CreateLink(var);
                        sceneVarLinks.Add(var.Link);
                    }
                }
            }
            if (complexSceneVars.Count < sceneVarLinks.Count)
            {
                List<SceneVar> links = new(sceneVarLinks);
                foreach (var var in links)
                {
                    if (complexSceneVars.Find(x => x.uniqueID == var.uniqueID) == null)
                    {
                        sceneVarLinks.Remove(var);
                    }
                }
            }
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
                default: return SceneVars;
            }
        }

        public List<SceneVar> Statics
        {
            get => sceneVars.FindAll(v => v.IsStatic);
        }
        public List<SceneVar> Modifyables
        {
            get => sceneVars.FindAll(v => !v.IsStatic);
        }
        public List<SceneVar> Listenables
        {
            get => SceneVars.FindAll(v => !v.IsStatic);
        }
        public List<SceneVar> Conditionable
        {
            get => SceneVars.FindAll(v => !v.IsStatic && v.type != SceneVarType.EVENT);
        }
        public List<SceneVar> Booleans
        {
            get => SceneVars.FindAll(v => v.type == SceneVarType.BOOL);
        }
        public List<SceneVar> Numbers
        {
            get => SceneVars.FindAll(v => v.type == SceneVarType.INT || v.type == SceneVarType.FLOAT);
        }
        public List<SceneVar> Integers
        {
            get => SceneVars.FindAll(v => v.type == SceneVarType.INT);
        }
        public List<SceneVar> Floats
        {
            get => SceneVars.FindAll(v => v.type == SceneVarType.FLOAT);
        }
        public List<SceneVar> Strings
        {
            get => SceneVars.FindAll(v => v.type == SceneVarType.STRING);
        }
        public List<SceneVar> Events
        {
            get => sceneVars.FindAll(v => v.type == SceneVarType.EVENT);
        }
        public List<SceneVar> NonEvents
        {
            get => SceneVars.FindAll(v => v.type != SceneVarType.EVENT);
        }
        #endregion

        #region Dependency
        public List<SceneVar> CleanListOfCycleDependencies(List<SceneVar> list, int UID)
        {
            List<SceneVar> sceneVars = new();
            foreach (SceneVar v in list)
            {
                if (!v.IsLink || complexSceneVars.Find(x => x.uniqueID == v.uniqueID).CanDependOn(UID))
                {
                    sceneVars.Add(v);
                }
            }
            return sceneVars;
        }
        #endregion
    }
}
