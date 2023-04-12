using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dhs5.Utility.SceneCreation
{
    [Serializable]
    public class SceneTimedCondition
    {
        public IEnumerator Condition()
        {
            yield break;
        }
    }
}
