using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dhs5.Utility.SceneCreation
{
    [CreateAssetMenu(fileName = "SceneVars", menuName = "Scene Creation/Scene Vars")]
    public class SceneVariablesSO : ScriptableObject
    {
        public List<SceneVar> sceneVars;
    }
}
