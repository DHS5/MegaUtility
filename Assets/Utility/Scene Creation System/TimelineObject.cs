using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;

namespace Dhs5.Utility.SceneCreation
{    
    [Serializable]
    public class TimelineObject : SceneState.ISceneVarSetupable
    {
        public string TimelineID { get; private set; }
        public int StepNumber { get; private set; }
        
        public SceneTimedCondition startCondition;
        public bool loop;
        public SceneLoopCondition endLoopCondition;
        
        // Action
        public List<SceneEvent> sceneEvents;
        public List<SceneTimelineEvent> sceneTimelineEvents;

        private IEnumerator startConditionCR;

        public void SetUp(SceneVariablesSO sceneVariablesSO)
        {
            sceneEvents.SetUp(sceneVariablesSO);
            sceneTimelineEvents.SetUp(sceneVariablesSO);
            
            startCondition.SetUp(sceneVariablesSO);
            endLoopCondition.SetUp(sceneVariablesSO);
        }
        
        public IEnumerator Process(SceneTimeline sceneTimeline, int step)
        {
            TimelineID = sceneTimeline.ID;
            StepNumber = step;
            
            // Start the end loop condition to be verified
            if (loop && endLoopCondition.TimedCondition)
                endLoopCondition.StartTimer();
            
            do
            {
                // Wait for the condition to be verified
                startConditionCR = startCondition.Condition();
                yield return StartCoroutine(startConditionCR);
                // Trigger Events
                Trigger();

            } while (loop && !endLoopCondition.CurrentConditionResult);
        }

        private void Trigger()
        {
            sceneEvents.Trigger();
            
            sceneTimelineEvents.Trigger(TimelineID);
        }

        #region Utility
        private IEnumerator StartCoroutine(IEnumerator Coroutine)
        {
            yield return SceneClock.Instance.StartCoroutine(Coroutine);
        }
        public void StopExecution()
        {
            SceneClock.Instance.StopCoroutine(startConditionCR);
        }
        #endregion
    }
}
