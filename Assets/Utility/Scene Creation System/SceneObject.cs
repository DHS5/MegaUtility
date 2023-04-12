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
                SceneEventManager.StartListening(listener.SceneVar.ID, OnEventReceived);
            }
        }
        private void OnDisable()
        {
            foreach (SceneListener listener in sceneListeners)
            {
                SceneEventManager.StopListening(listener.SceneVar.ID, OnEventReceived);
            }
        }

        private void OnEventReceived(SceneVar var)
        {
            SceneListener listener = GetListenerByID(var.ID);
            if (listener.VerifyCondition())
            {
                listener.events.Invoke(var);
                DebugListener(listener);
            }
        }

        private SceneListener GetListenerByID(string varID)
        {
            return sceneListeners.Find(l => l.SceneVar.ID == varID);
        }
        #endregion

        #region Update Listeners & Actions
        private void OnValidate()
        {
            UpdateSceneVariables();
        }

        private void UpdateSceneVariables()
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
        public void TriggerSceneAction(string actionID)
        {
            SceneAction action = sceneActions.Find(a => a.actionID == actionID);
            action.Trigger();
            DebugAction(action);
        }
        #endregion

        #region Debug
        [Header("Debug")]
        public bool debugListeners;
        public bool debugActions;

        private void DebugListener(SceneListener listener)
        {
            if (debugListeners)
                Debug.Log("Received event : " + listener.SceneVar.ID + " = " + listener.SceneVar.Value);
        }
        private void DebugAction(SceneAction action)
        {
            if (debugActions)
                Debug.Log("Triggered action : " + action.actionID);
        }
        #endregion
    }
}
