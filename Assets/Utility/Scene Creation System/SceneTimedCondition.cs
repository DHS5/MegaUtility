using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dhs5.Utility.SceneCreation
{
    [Serializable]
    public class SceneTimedCondition
    {
        public bool timedCondition = false;
        public bool CurrentConditionResult { get; private set; }
        public IEnumerator Condition()
        {
            yield break;
            CurrentConditionResult = true;
        }
    }
}
