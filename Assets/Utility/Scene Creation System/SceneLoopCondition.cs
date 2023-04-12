using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dhs5.Utility.SceneCreation
{
    [Serializable]
    public class SceneLoopCondition
    {
        public bool timedCondition;
        public float startTime;
        public bool CurrentConditionResult { get; private set; }

        public void StartTimer()
        {
            startTime = Time.time;
        }
    }
}
