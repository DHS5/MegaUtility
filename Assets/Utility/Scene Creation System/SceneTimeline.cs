using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dhs5.Utility.SceneCreation
{
    [Serializable]
    public class SceneTimeline
    {
        [HideInInspector] public SceneVariablesSO sceneVariablesSO;

        public List<TimelineObject> timelineObjects;
    }
}
