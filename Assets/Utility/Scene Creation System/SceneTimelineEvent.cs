using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Dhs5.Utility.SceneCreation
{
    [Serializable]
    public class SceneTimelineEvent: SceneState.ISceneVarSetupable
    {
        public List<SceneCondition> sceneConditions;
        public UnityEvent<TimelineEventParam> events;
        public TimelineEventParam param;
    
        public void SetUp(SceneVariablesSO sceneVariablesSO)
        {
            sceneConditions.SetUp(sceneVariablesSO);
        }
    
        public void Trigger(string timelineID, int step)
        {
            events?.Invoke(param.Send(timelineID, step));
        }
    }
}
