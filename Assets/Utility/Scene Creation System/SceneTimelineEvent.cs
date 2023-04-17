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

    public void Trigger(string timelineID)
    {
        if (!String.IsNullOrWhiteSpace(param.timelineID)) events?.Invoke(param);
        // If the param timelineID is blank then use parent timelineID
        else events?.Invoke(param.Send(timelineID));
    }
    }
}
