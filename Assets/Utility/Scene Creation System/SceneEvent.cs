using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace Dhs5.Utility.SceneCreation
{
    [Serializable]
    public class SceneEvent
    {
        public string eventID;
        [Space(5)]

        public List<SceneCondition> sceneConditions;

        public List<SceneAction> sceneActions;

        public List<SceneParameteredEvent> sceneParameteredEvents;

        public bool debug = false;

        public void Trigger()
        {
            if (sceneConditions != null)
            {
                if (!sceneConditions.VerifyConditions()) return;
            }

            if (sceneActions != null)
            {
                foreach (var action in sceneActions)
                    action.Trigger();
            }

            if (sceneParameteredEvents != null)
            {
                foreach (var paramEvent in sceneParameteredEvents)
                    paramEvent.Trigger();
            }

            if (debug)
                DebugSceneEvent();
        }

        

        public void SetUp(SceneVariablesSO _sceneVariablesSO)
        {
            if (sceneConditions != null)
            {
                foreach (var condition in sceneConditions)
                    condition.sceneVariablesSO = _sceneVariablesSO;
            }
            if (sceneActions != null)
            {
                foreach (var action in sceneActions)
                    action.sceneVariablesSO = _sceneVariablesSO;
            }
            if (sceneParameteredEvents != null)
            {
                foreach (var e in sceneParameteredEvents)
                    e.SetUp(_sceneVariablesSO);
            }
        }

        private void DebugSceneEvent()
        {
            ZDebug.LogE("Triggered Scene Event : ", eventID);
        }
    }
}
