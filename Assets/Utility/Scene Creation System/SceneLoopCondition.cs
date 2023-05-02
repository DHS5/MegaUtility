using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dhs5.Utility.SceneCreation
{
    [Serializable]
    public class SceneLoopCondition
    {
        public enum LoopConditionType
        {
            SCENE = 0,
            TIMED = 1,
            ITERATION = 2,
        }

        public LoopConditionType conditionType;
        
        public SceneVarTween timeToWait;
        public SceneVarTween iterationNumber;
        public List<SceneCondition> sceneConditions;
        
        
        private float startTime;
        private int currentIteration = 0;
        
        
        public bool TimedCondition
        {
            get => conditionType == LoopConditionType.TIMED;
        }

        public bool CurrentConditionResult
        {
            get
            {
                switch (conditionType)
                {
                    case LoopConditionType.TIMED:
                        return Time.time - startTime >= timeToWait.FloatValue;
                    case LoopConditionType.SCENE:
                        return sceneConditions.VerifyConditions();
                    case LoopConditionType.ITERATION:
                        currentIteration++;
                        return currentIteration >= iterationNumber.IntValue;
                    default:
                        return true;
                }
            }
        }

        public void SetUp(SceneVariablesSO sceneVariablesSO)
        {
            sceneConditions.SetUp(sceneVariablesSO);
            timeToWait.SetUp(sceneVariablesSO, SceneVarType.FLOAT, true);
            iterationNumber.SetUp(sceneVariablesSO, SceneVarType.INT, true);
        }

        public void StartTimer()
        {
            startTime = Time.time;
        }

        public void Reset()
        {
            currentIteration = 0;
            startTime = Time.time;
        }
    }
}
