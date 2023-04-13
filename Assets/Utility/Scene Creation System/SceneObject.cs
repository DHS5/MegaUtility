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
        public List<SceneAction> sceneActions;

        #region Scene Events subscription
        private void OnEnable()
        {
            foreach (SceneListener listener in sceneListeners)
            {
                listener.Register();
            }
            RegisterTweens();
        }
        private void OnDisable()
        {
            foreach (SceneListener listener in sceneListeners)
            {
                listener.Unregister();
            }
            UnregisterTweens();
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
        public bool debugActions;
        private void DebugAction(SceneAction action)
        {
            if (debugActions)
                Debug.Log("Triggered action : " + action.actionID);
        }
        #endregion
    }
}
