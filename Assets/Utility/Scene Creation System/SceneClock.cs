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
        
        [Header("Timelines")]
        public List<SceneTimeline> sceneTimelines;

        #region Update SceneVariables
        protected override void UpdateSceneVariables()
        {
            base.UpdateSceneVariables();

            sceneTimelines.SetUp(sceneVariablesSO);
        }
        #endregion

        #region Listener functions
        public void StartTimeline(TimelineEventParam param)
        {
            StartTimeline(param.timelineID, param.timelineStep);
        }
        public void StartTimeline(string timelineID, int step = 0)
        {
            sceneTimelines.Find(t => t.ID == timelineID)?.Start(step);
        }
        public void StartTimeline(string timelineID) { StartTimeline(timelineID, 0); }
        public void StopTimeline(TimelineEventParam param)
        {
            StopTimeline(param.timelineID);
        }
        public void StopTimeline(string timelineID)
        {
            sceneTimelines.Find(t => t.ID == timelineID)?.Stop();
        }
        public void GoToStep(TimelineEventParam param)
        {
            sceneTimelines.Find(t => t.ID == param.timelineID)?.GoToStep(param.timelineStep);
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

        public TimelineEventParam Send(string ID)
        {
            timelineID = ID;
            return this;
        }
        
        [Tooltip("Leave blank to call parent timeline")]
        public string timelineID;
        [Tooltip("Index of the step in the TimelineObjects list")]
        public int timelineStep;
    }
}
