using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Dhs5.Utility.SceneCreation
{
    public class SceneClock : SceneObject
    {
        #region Singleton

        public static SceneClock Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                return;
            }
            Destroy(gameObject);
        }

        #endregion
        
        [Header("Scene Timelines")]
        public List<SceneTimeline> sceneTimelines;

        protected override void UpdateSceneVariables()
        {
            base.UpdateSceneVariables();

            foreach (var timeline in sceneTimelines)
            {
                timeline.sceneVariablesSO = sceneVariablesSO;
            }
        }

        #region Listener functions
        public void StartTimeline(TimelineEventParam param)
        {
            StartTimeline(param.timelineID, param.timelineStep);
        }
        public void StartTimeline(string timelineID, int step = 0)
        {
            sceneTimelines.Find(t => t.ID == timelineID).Start(step);
        }
        public void StartTimeline(string timelineID) { StartTimeline(timelineID); }
        public void StopTimeline(TimelineEventParam param)
        {
            StopTimeline(param.timelineID);
        }
        public void StopTimeline(string timelineID)
        {
            sceneTimelines.Find(t => t.ID == timelineID).Stop();
        }
        public void TimelineGoToStep(TimelineEventParam param)
        {
            sceneTimelines.Find(t => t.ID == param.timelineID).GoToStep(param.timelineStep);
        }
        public void TimelineGoToStep(TimelineEventParam param, int step)
        {
            sceneTimelines.Find(t => t.ID == param.timelineID).GoToStep(param.timelineStep);
        }
        #endregion
    }

    [Serializable]
    public struct TimelineEventParam
    {
        public TimelineEventParam(string _timelineID, int _step = 0)
        {
            timelineID = _timelineID;
            timelineStep = _step;
        }

        public string timelineID;
        public int timelineStep;
    }
}
