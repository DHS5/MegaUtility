using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dhs5.Utility.SceneCreation
{
    [Serializable]
    public class SceneTimedCondition
    {
        [Serializable]
        public enum TimedConditionType
        {
            WAIT_FOR_TIME = 0,
            WAIT_UNTIL_SCENE_CONDITION = 1,
            WAIT_WHILE_SCENE_CONDITION = 2,
        }

        public TimedConditionType conditionType;
        
        public float timeToWait;
        public List<SceneCondition> sceneConditions;
        
        public void SetUp(SceneVariablesSO sceneVariablesSO)
        {
            sceneConditions.SetUp(sceneVariablesSO);
        }
        
        public IEnumerator Condition()
        {
            switch (conditionType)
            {
                case TimedConditionType.WAIT_FOR_TIME:
                    yield return new WaitForSeconds(timeToWait);
                    break;
                case TimedConditionType.WAIT_UNTIL_SCENE_CONDITION:
                    yield return new WaitUntil(sceneConditions.VerifyConditions);
                    break;
                case TimedConditionType.WAIT_WHILE_SCENE_CONDITION:
                    yield return new WaitWhile(sceneConditions.VerifyConditions);
                    break;
            }
            
            yield break;
        }
    }
}
