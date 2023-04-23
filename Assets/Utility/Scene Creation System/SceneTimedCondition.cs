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
        
        public SceneVarTween timeToWait;
        public List<SceneCondition> sceneConditions;
        
        public void SetUp(SceneVariablesSO sceneVariablesSO)
        {
            sceneConditions.SetUp(sceneVariablesSO);
            timeToWait.SetUp(sceneVariablesSO, SceneVarType.FLOAT);
        }
        
        public IEnumerator Condition()
        {
            startTime = Time.time;
            stop = false;
            switch (conditionType)
            {
                case TimedConditionType.WAIT_FOR_TIME:
                    //yield return new WaitForSeconds(timeToWait);
                    yield return new WaitUntil(TimeIsUp);
                    break;
                case TimedConditionType.WAIT_UNTIL_SCENE_CONDITION:
                    //yield return new WaitUntil(sceneConditions.VerifyConditions);
                    yield return new WaitUntil(SceneConditionVerified);
                    break;
                case TimedConditionType.WAIT_WHILE_SCENE_CONDITION:
                    //yield return new WaitWhile(sceneConditions.VerifyConditions);
                    yield return new WaitWhile(SceneConditionUnverified);
                    break;
            }

            stop = false;
            yield break;
        }


        private bool stop = false;
        public void BreakCoroutine()
        {
            stop = true;
        }

        private float startTime;
        private bool TimeIsUp()
        {
            return stop || (Time.time - startTime >= timeToWait.FloatValue);
        }

        private bool SceneConditionVerified()
        {
            return stop || sceneConditions.VerifyConditions();
        }

        private bool SceneConditionUnverified()
        {
            return !stop && sceneConditions.VerifyConditions();
        }
    }
}
