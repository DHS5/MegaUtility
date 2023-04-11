using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace Dhs5.Utility.SceneCreation
{
    [Serializable]
    public class SceneListener
    {
        [HideInInspector] public SceneVariablesSO sceneVariablesSO;

        [HideInInspector] public int varIndex;
        public SceneVar SceneVar { get { return sceneVariablesSO.sceneVars[varIndex]; } }
        public UnityEvent<SceneVar> events;
    }
}
