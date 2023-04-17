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

        private void Start()
        {
            StartTimeline("Timeline1");
        }

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
            StartTimeline(param.GetParamTimelineID, param.GetParamTimelineStep);
        }
        public void StartTimeline(string timelineID, int step = 0)
        {
            sceneTimelines.Find(t => t.ID == timelineID)?.Start(step);
        }
        public void StartTimeline(string timelineID) { StartTimeline(timelineID, 0); }
        public void StopTimeline(TimelineEventParam param)
        {
            StopTimeline(param.GetParamTimelineID);
        }
        public void StopTimeline(string timelineID)
        {
            sceneTimelines.Find(t => t.ID == timelineID)?.Stop();
        }
        public void GoToStep(TimelineEventParam param)
        {
            sceneTimelines.Find(t => t.ID == param.GetParamTimelineID)?.
                StartOrGoTo(param.GetParamTimelineStep);
        }
        #endregion
        
        #region Debug
        public void DebugTest(TimelineEventParam param)
        {
            ZDebug.LogE("", param.SenderTimelineID, " in step ", param.SenderTimelineStep, " sent event : ",  param.GetParamTimelineID + ", step : " + param.GetParamTimelineStep + ", time : " + Time.time);
        }
        #endregion
    }

    #region Timeline Event Param
    [Serializable]
    public struct TimelineEventParam
    {
        public TimelineEventParam Send(string ID, int step)
        {
            SenderTimelineID = ID;
            SenderTimelineStep = step;
            return this;
        }
        
        public string SenderTimelineID { get; private set; }
        public int SenderTimelineStep { get; private set; }
        
        [Tooltip("Leave blank to call parent timeline")]
        public string timelineID;
        [Tooltip("Set to -1 to call own step number")]
        public int timelineStep;

        public string GetParamTimelineID
        {
            get => String.IsNullOrWhiteSpace(timelineID) ? SenderTimelineID : timelineID;
        }

        public int GetParamTimelineStep
        {
            get => timelineStep == -1 ? SenderTimelineStep : timelineStep;
        }
    }
    
    #endregion
}
