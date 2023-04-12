using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;

namespace Dhs5.Utility.SceneCreation
{    
    [Serializable]
    public class TimelineObject
    {
        public SceneTimedCondition startCondition;
        public bool loop;
        public SceneLoopCondition endLoopCondition;
        
        // Action
        public UnityEvent<TimelineEventParam> events;

        public IEnumerator Process(SceneTimeline sceneTimeline, int step)
        {
            // Start the end loop condition to be verified
            if (loop && endLoopCondition.timedCondition)
                endLoopCondition.StartTimer();
            
            do
            {
                // Wait for the condition to be verified
                yield return StartCoroutine(startCondition.Condition());
                // Trigger Events
                Action(sceneTimeline, step);

            } while (loop && endLoopCondition.CurrentConditionResult);
        }

        private void Action(SceneTimeline sceneTimeline, int step)
        {
            events?.Invoke(new TimelineEventParam(sceneTimeline.ID, step));
        }

        #region Utility
        private IEnumerator StartCoroutine(IEnumerator Coroutine)
        {
            yield return SceneClock.Instance.StartCoroutine(Coroutine);
        }
        #endregion
    }
}
