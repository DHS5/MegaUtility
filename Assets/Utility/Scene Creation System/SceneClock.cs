using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
