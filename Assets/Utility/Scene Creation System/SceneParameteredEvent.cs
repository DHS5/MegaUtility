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
        public SceneVariablesSO sceneVariablesSO;

        [Serializable]
        public enum ParameterType
        {
            SCENE_PARAM = 0,
            CONDITION_PARAM = 1,
        }

        public UnityEvent<object> events;

        public ParameterType parameterType;

        public int paramUniqueID;
        public List<SceneCondition> conditionsParam;

        public float propertyHeight;

        public void SetUp(SceneVariablesSO _sceneVariablesSO)
        {
            sceneVariablesSO = _sceneVariablesSO;

            if (conditionsParam != null)
            {
                foreach (var condition in conditionsParam)
                    condition.sceneVariablesSO = _sceneVariablesSO;
            }
        }

        public void Trigger()
        {
            events?.Invoke(parameterType == ParameterType.SCENE_PARAM ? SceneState.GetObjectValue(paramUniqueID) : VerifyConditions());
        }

        private bool VerifyConditions()
        {
            if (conditionsParam != null)
            {
                foreach (var condition in conditionsParam)
                    if (!condition.VerifyCondition()) return false;
            }
            return true;
        }
    }
}
