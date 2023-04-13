using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SceneCreation
{
    public class EventParam2
    {
        string sceneObjectID;
        SceneVar sceneVar;
        // Type : change, has_changed, init
        private string triggerObjectID;
    }

    public static class SceneEventManager
    {
        private static Dictionary<int, Action<SceneVar>> eventDico = new();


        public static void StartListening(int keyEvent, Action<SceneVar> listener)
        {
            if (eventDico.ContainsKey(keyEvent))
            {
                eventDico[keyEvent] += listener;
            }
            else
            {
                eventDico.Add(keyEvent, listener);
            }
        }

        public static void StopListening(int keyEvent, Action<SceneVar> listener)
        {
            if (eventDico.ContainsKey(keyEvent))
            {
                eventDico[keyEvent] -= listener;
            }
        }

        public static void TriggerEvent(int keyEvent, SceneVar param)
        {
            if (eventDico.TryGetValue(keyEvent, out Action<SceneVar> thisEvent))
            {
                thisEvent.Invoke(param);
            }
        }
    }
}
