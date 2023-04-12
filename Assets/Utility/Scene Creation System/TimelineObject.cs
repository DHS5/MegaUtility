using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;

namespace Dhs5.Utility.SceneCreation
{
    [Serializable]
    public enum TimelineObjectType
    {
        ACTION,
        SEQUENCE,
        PARALLEL,
    }
    
    [Serializable]
    public class TimelineObject
    {
        public TimelineObjectType type;

        public SceneTimedCondition startCondition;
        public bool loop;
        public SceneTimedCondition endLoopCondition;
        
        // Action
        public UnityEvent<EventParam> events;
        
        // Sequence / Parallel
        public List<TimelineObject> timelineObjects;

        public IEnumerator Process(Action onComplete = null)
        {
            // Start the end loop condition to be verified
            if (loop && endLoopCondition.timedCondition)
                StartCoroutine(endLoopCondition.Condition());
            
            do
            {
                // Wait for the condition to be verified
                yield return StartCoroutine(startCondition.Condition());
                // Wait for the Action to be complete
                yield return StartCoroutine(Action());
            } while (loop && endLoopCondition.CurrentConditionResult);
            
            onComplete?.Invoke();
        }

        #region Actions
        private IEnumerator Action()
        {
            switch (type)
            {
                case TimelineObjectType.ACTION:
                    SimpleAction();
                    yield break;
                case TimelineObjectType.SEQUENCE:
                    yield return StartCoroutine(SequenceAction());
                    break;
                case TimelineObjectType.PARALLEL:
                    yield return StartCoroutine(ParallelAction());
                    break;
            }
        }

        private void SimpleAction()
        {
            // Trigger events
        }

        private IEnumerator SequenceAction()
        {
            foreach (var timelineObj in timelineObjects)
            {
                yield return timelineObj.Process();
            }
        }
        private IEnumerator ParallelAction()
        {
            int timelineObjectsFinished = 0;
            int timelineObjectsToFinish = 0;
            foreach (var timelineObj in timelineObjects)
            {
                timelineObj.Process(() => timelineObjectsFinished++);
                timelineObjectsToFinish++;
            }

            yield return new WaitUntil(() => timelineObjectsFinished == timelineObjectsToFinish);
        }

        #endregion

        #region Utility
        private IEnumerator StartCoroutine(IEnumerator Coroutine)
        {
            yield return SceneClock.Instance.StartCoroutine(Coroutine);
        }
        #endregion
    }
}
