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

        public List<SceneAction> sceneActions;

        public List<SceneParameteredEvent> sceneParameteredEvents;

        public void Trigger(SceneVar var)
        {
            
        }

        

        public void SetUp(SceneVariablesSO sceneVariablesSO)
        {
            if (sceneActions != null)
            {
                foreach (var action in sceneActions)
                    action.sceneVariablesSO = sceneVariablesSO;
            }
            if (sceneParameteredEvents != null)
            {
                foreach (var e in sceneParameteredEvents)
                    e.SetUp(sceneVariablesSO);
            }
        }
    }
}
