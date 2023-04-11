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

        #region Scene Events subscription
        private void OnEnable()
        {
            foreach (SceneListener listener in sceneListeners)
            {
                SceneEventManager.StartListening(listener.sceneVar.ID, OnEventReceived);
            }
        }
        private void OnDisable()
        {
            foreach (SceneListener listener in sceneListeners)
            {
                SceneEventManager.StopListening(listener.sceneVar.ID, OnEventReceived);
            }
        }

        private void OnEventReceived(SceneVar var)
        {
            GetEventByID(var.ID).Invoke(var);
        }

        private UnityEvent<SceneVar> GetEventByID(string varID)
        {
            return sceneListeners.Find(l => l.sceneVar.ID == varID).events;
        }
        #endregion

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
        }
    }
}
