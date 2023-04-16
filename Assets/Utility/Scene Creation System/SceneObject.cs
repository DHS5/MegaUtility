using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Dhs5.Utility.SceneCreation
{
    [DisallowMultipleComponent]
    public class SceneObject : MonoBehaviour
    {
        public SceneVariablesSO sceneVariablesSO;

        [Header("Listeners")]
        public List<SceneListener> sceneListeners;

        [Header("Actions")]
        public List<SceneEvent> sceneEvents;

        #region Scene Events subscription
        private void OnEnable()
        {
            if (sceneListeners != null)
            {
                foreach (SceneListener listener in sceneListeners)
                {
                    listener.Register();
                }
            }
        }
        private void OnDisable()
        {
            if (sceneListeners != null)
            {
                foreach (SceneListener listener in sceneListeners)
                {
                    listener.Unregister();
                }
            }
        }

        private List<SceneListener> GetListenersByID(int varUniqueID)
        {
            return sceneListeners.FindAll(l => l.CurrentSceneVar.uniqueID == varUniqueID);
        }
        #endregion

        #region Update Listeners, Actions & Tweens
        private void OnValidate()
        {
            UpdateSceneVariables();
        }

        protected virtual void UpdateSceneVariables()
        {
            if (sceneListeners != null)
            {
                foreach (SceneListener listener in sceneListeners)
                {
                    listener.SetUp(sceneVariablesSO);
                }
            }

            if (sceneEvents != null)
            {
                foreach (SceneEvent sceneEvent in sceneEvents)
                {
                    sceneEvent.SetUp(sceneVariablesSO);
                }
            }
        }

        #endregion

        #region Trigger Action

        protected List<SceneEvent> GetSceneEventsByID(string eventID)
        {
            if (sceneEvents == null) return null;
            return sceneEvents.FindAll(a => a.eventID == eventID);
        }
        public void TriggerSceneEvent(string eventID)
        {
            List<SceneEvent> events = GetSceneEventsByID(eventID);
            if (events == null) return;
            foreach (var sceneEvent in events)
            {
                sceneEvent.Trigger();
            }
        }
        #endregion
    }
}
