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

        public SceneVar sceneVar;
        public UnityEvent<SceneVar> events;
    }
}
