using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dhs5.Utility.SceneCreation
{
    [Serializable]
    public class SceneTimeline : SceneState.ISceneVarSetupable
    {
        public string ID;
        public bool loop;
        public SceneLoopCondition endLoopCondition;
        public List<TimelineObject> timelineObjects;

        public bool IsActive { get; private set; }

        private Coroutine coroutine;
        private Queue<TimelineObject> timelineQueue = new();
        private TimelineObject currentTimelineObject;
        
        public void SetUp(SceneVariablesSO sceneVariablesSO)
        {
            endLoopCondition.SetUp(sceneVariablesSO);
            timelineObjects.SetUp(sceneVariablesSO);
        }

        private IEnumerator TimelineRoutine(int step = 0)
        {
            IsActive = true;

            do
            {
                for (;;)
                {
                    if (timelineQueue.Count < 1)
                    {
                        break;
                    }
                    currentTimelineObject = timelineQueue.Dequeue();
                    yield return StartCoroutine(currentTimelineObject.Process(this, 0));
                }
            } while (loop && !endLoopCondition.CurrentConditionResult);

            IsActive = false;
        }
        
        #region Timeline Actions
        public void Start(int step = 0)
        {
            if (IsActive) return;
            
            SetUpQueue(step);
            coroutine = StartMainCR(TimelineRoutine(step));
        }
        public void Stop()
        {
            if (!IsActive) return;
            
            StopMainCR();
            currentTimelineObject.StopExecution();

            IsActive = false;
        }
        public void GoToStep(int step)
        {
            SetUpQueue(step);
        }
        public void StartOrGoTo(int step)
        {
            if (IsActive)
            {
                GoToStep(step);
                return;
            }
            Start(step);
        }
        #endregion
        
        #region Utility
        private void SetUpQueue(int step = 0)
        {
            timelineQueue = new();

            for (int i = step; i < timelineObjects.Count; i++)
            {
                timelineQueue.Enqueue(timelineObjects[i]);
            }
        }
        
        private Coroutine StartMainCR(IEnumerator Coroutine)
        {
            return SceneClock.Instance.StartCoroutine(Coroutine);
        }
        private void StopMainCR()
        {
            SceneClock.Instance.StopCoroutine(coroutine);
        }

        private IEnumerator StartCoroutine(IEnumerator Coroutine)
        {
            yield return SceneClock.Instance.StartCoroutine(Coroutine);
        }
        #endregion
    }
}
