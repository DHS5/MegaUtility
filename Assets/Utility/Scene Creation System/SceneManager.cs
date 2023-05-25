using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace Dhs5.Utility.SceneCreation
{
    public class SceneManager : MonoBehaviour
    {
        public SceneVariablesSO sceneVariablesSO;

        void Start()
        {
            SceneState.SetSceneVars(sceneVariablesSO);
        }

        #region SceneClock Actions
        [Preserve]
        public void StartTimeline(string timelineID)
        {
            SceneClock.Instance.StartTimeline(timelineID);
        }
        [Preserve]
        public void StartTimeline(string timelineID, int step)
        {
            SceneClock.Instance.StartTimeline(timelineID, step);
        }
        [Preserve]
        public void StopTimeline(string timelineID)
        {
            SceneClock.Instance.StopTimeline(timelineID);
        }
        [Preserve]
        public void TimelineGoToStep(string timelineID, int step, bool interrupt)
        {
            SceneClock.Instance.GoToStep(timelineID, step, interrupt);
        }
        #endregion
    }
}
