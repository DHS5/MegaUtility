using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace Dhs5.Utility.SceneCreation
{
    [Serializable]
    public class SceneParameteredEvent2 : SceneState.ISceneVarSetupable
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

            conditionsParam.SetUp(sceneVariablesSO);
        }

        public void Trigger()
        {
            events?.Invoke(parameterType == ParameterType.SCENE_PARAM ? SceneState.GetObjectValue(paramUniqueID) : VerifyConditions());
        }

        private bool VerifyConditions()
        {
            return conditionsParam.VerifyConditions();
        }
    }
}
