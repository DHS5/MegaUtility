using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dhs5.Utility.SceneCreation
{
    [CreateAssetMenu(fileName = "SceneVars", menuName = "Scene Creation/Scene Vars")]
    public class SceneVariablesSO : ScriptableObject
    {
        public List<SceneVar> sceneVars;

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
            int uniqueID = 0;
            do
            {
                uniqueID = Random.Range(1, 10000);
            } while (UniqueIDs.Contains(uniqueID));
            
            return uniqueID;
        }
    }
}
