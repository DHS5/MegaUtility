using System;
using System.Collections;
using System.Collections.Generic;
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
        private bool executing;
        private bool canInterrupt;

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
            
            // Reset the end loop condition
            endLoopCondition.Reset();
            
            do
            {
                executing = true;
                
                // Wait for the condition to be verified
                startConditionCR = startCondition.Condition();
                yield return StartCoroutine(startConditionCR);

                if (executing || !canInterrupt)
                {
                    // Trigger Events
                    Trigger();
                }

            } while (loop && !endLoopCondition.CurrentConditionResult && executing);
        }

        private void Trigger()
        {
            sceneEvents.Trigger();
            
            sceneTimelineEvents.Trigger(TimelineID, StepNumber);
        }

        #region Utility
        private IEnumerator StartCoroutine(IEnumerator Coroutine)
        {
            yield return SceneClock.Instance.StartCoroutine(Coroutine);
        }
        public void StopExecution(bool interrupt)
        {
            executing = false;
            canInterrupt = interrupt;
            if (interrupt) startCondition.BreakCoroutine();
        }
        public void StopCoroutine()
        {
            SceneClock.Instance.StopCoroutine(startConditionCR);
        }
        #endregion
    }
}
