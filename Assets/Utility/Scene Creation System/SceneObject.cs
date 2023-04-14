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
                    listener.sceneVariablesSO = sceneVariablesSO;
                }
            }

            if (sceneActions != null)
            {
                foreach (SceneAction action in sceneActions)
                {
                    action.sceneVariablesSO = sceneVariablesSO;
                }
            }
        }

        #endregion

        #region Trigger Action

        protected List<SceneAction> GetSceneActionsByID(string actionID)
        {
            if (sceneActions != null) return null;
            return sceneActions.FindAll(a => a.actionID == actionID);
        }
        public void TriggerSceneAction(string actionID)
        {
            List<SceneAction> actions = GetSceneActionsByID(actionID);
            if (actions == null) return;
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
