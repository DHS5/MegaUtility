using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace Dhs5.Utility.SceneCreation
{
    [Serializable]
    public class SceneParameteredEvent
    {
        [Serializable]
        public enum ParameterType
        {
            SCENE_PARAM = 0,
            CONDITION_PARAM = 1,
        }

        public UnityEvent<object> events;

        public ParameterType parameterType;

        public int paramUniqueID;
        public SceneCondition conditionParam;

        public void SetUp(SceneVariablesSO sceneVariablesSO)
        {
            conditionParam.sceneVariablesSO = sceneVariablesSO;
        }

        public void Trigger()
        {
            events?.Invoke(parameterType == ParameterType.SCENE_PARAM ? SceneState.GetObjectValue(paramUniqueID) : conditionParam.VerifyCondition());
        }
    }
}
