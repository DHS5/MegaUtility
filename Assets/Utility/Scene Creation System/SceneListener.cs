using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace Dhs5.Utility.SceneCreation
{
    [Serializable]
    public class SceneListener
    {
        public SceneVariablesSO sceneVariablesSO;

        // SceneVar selection
        public int varUniqueID;
        public SceneVar CurrentSceneVar
        {
            get { return SceneState.GetSceneVar(varUniqueID); }
        }
        
        // Condition
        public bool hasCondition;

        public List<SceneCondition> conditions;
        
        public UnityEvent<SceneVar> events;

        public bool debug = false;
        public float propertyHeight;

        #region Event Subscription
        public void Register()
        {
            SceneEventManager.StartListening(CurrentSceneVar.uniqueID, OnListenerEvent);
        }
        public void Unregister()
        {
            SceneEventManager.StopListening(CurrentSceneVar.uniqueID, OnListenerEvent);
        }
        private void OnListenerEvent(SceneVar var)
        {
            if (VerifyConditions())
            {
                events.Invoke(var);
                if (debug)
                    Debug.Log("Received event : " + CurrentSceneVar.ID + " = " + CurrentSceneVar.Value);
            }
        }
        #endregion


        public void SetUp(SceneVariablesSO _sceneVariablesSO)
        {
            sceneVariablesSO = _sceneVariablesSO;

            if (conditions != null)
            {
                foreach (var condition in conditions)
                    condition.sceneVariablesSO = _sceneVariablesSO;
            }
        }

        public bool VerifyConditions()
        {
            if (!hasCondition) return true;

            if (conditions != null)
            {
                return conditions.VerifyConditions();
            }

            return true;
        }
    }
}
