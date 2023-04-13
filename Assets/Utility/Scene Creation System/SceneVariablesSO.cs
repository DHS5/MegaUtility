using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneCreation
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
    }
}
