using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dhs5.Utility.SceneCreation
{
    public class SceneManager : MonoBehaviour
    {
        public SceneVariablesSO sceneVariablesSO;

        void Awake()
        {
            SceneState.SetSceneVars(sceneVariablesSO);
        }
    }
}
