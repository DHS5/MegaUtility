using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace Dhs5.Utility.SceneCreation
{
    [Serializable]
    public class SceneListener : SceneState.ISceneVarSetupable
    {
        public SceneVariablesSO sceneVariablesSO;

        // SceneVar selection
        [SerializeField] private int varUniqueID;
        public int UID => varUniqueID;
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
            SceneEventManager.StartListening(varUniqueID, OnListenerEvent);
        }
        public void Unregister()
        {
            SceneEventManager.StopListening(varUniqueID, OnListenerEvent);
        }
        private void OnListenerEvent(SceneVar var)
        {
            if (VerifyConditions())
            {
                events.Invoke(var);
                if (debug)
                    Debug.LogError("Received event : " + CurrentSceneVar.ToString());
            }
        }
        #endregion


        public void SetUp(SceneVariablesSO _sceneVariablesSO)
        {
            sceneVariablesSO = _sceneVariablesSO;

            conditions.SetUp(sceneVariablesSO);
        }

        public bool VerifyConditions()
        {
            return !hasCondition || conditions.VerifyConditions();
        }
    }
}
