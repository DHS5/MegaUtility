using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SceneCreation
{
    [DisallowMultipleComponent]
    public class SceneObject : MonoBehaviour
    {
        public SceneVariablesSO sceneVariablesSO;

        [Header("Listeners")]
        public List<SceneListener> sceneListeners;

        [Header("Actions")]
        public List<SceneAction> sceneActions;

        #region Scene Events subscription
        private void OnEnable()
        {
            foreach (SceneListener listener in sceneListeners)
            {
                SceneEventManager.StartListening(listener.SceneVar.uniqueID, OnEventReceived);
            }
            RegisterTweens();
        }
        private void OnDisable()
        {
            foreach (SceneListener listener in sceneListeners)
            {
                SceneEventManager.StopListening(listener.SceneVar.uniqueID, OnEventReceived);
            }

            UnregisterTweens();
        }

        private void OnEventReceived(SceneVar var)
        {
            List<SceneListener> listeners = GetListenersByID(var.uniqueID);
            foreach (var listener in listeners)
            {
                if (listener.VerifyCondition())
                {
                    listener.events.Invoke(var);
                    DebugListener(listener);
                }
            }
        }

        private List<SceneListener> GetListenersByID(int varUniqueID)
        {
            return sceneListeners.FindAll(l => l.SceneVar.uniqueID == varUniqueID);
        }
        protected virtual void RegisterTweens() {} // SceneEventManager.StartListening(tween.ID, tween.OnEventReceived)
        protected virtual void UnregisterTweens() {}
        #endregion

        #region Update Listeners, Actions & Tweens
        private void OnValidate()
        {
            UpdateSceneVariables();
        }

        protected virtual void UpdateSceneVariables()
        {
            foreach (SceneListener listener in sceneListeners)
            {
                listener.sceneVariablesSO = sceneVariablesSO;
            }
            foreach (SceneAction action in sceneActions)
            {
                action.sceneVariablesSO = sceneVariablesSO;
            }
        }

        #endregion

        #region Trigger Action

        protected List<SceneAction> GetSceneActionsByID(string actionID)
        {
            return sceneActions.FindAll(a => a.actionID == actionID);
        }
        public void TriggerSceneAction(string actionID)
        {
            List<SceneAction> actions = GetSceneActionsByID(actionID);
            foreach (var action in actions)
            {
                DebugAction(action);
                action.Trigger();
            }
        }
        #endregion

        #region Debug
        [Header("Debug")]
        public bool debugListeners;
        public bool debugActions;

        private void DebugListener(SceneListener listener)
        {
            if (debugListeners)
                Debug.LogError("Received event : " + listener.SceneVar.ID + " = " + listener.SceneVar.Value);
        }
        private void DebugAction(SceneAction action)
        {
            if (debugActions)
                Debug.LogError("Triggered action : " + action.actionID);
        }
        #endregion
    }
}
