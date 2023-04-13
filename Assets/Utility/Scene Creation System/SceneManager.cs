using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneCreation
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
