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

        public string ID;
        public bool loop;
        public List<TimelineObject> timelineObjects;


        public bool IsActive { get; private set; }
        public void Start(int step = 0)
        {
            if (IsActive) return;

            IsActive = true;
        }
        public void Stop()
        {
            if (!IsActive) return;

            IsActive = false;
        }
        public void GoToStep(int step)
        {

        }
    }
}
