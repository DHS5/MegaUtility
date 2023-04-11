using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Dhs5.Utility.SceneCreation
{
    public class EventParam
    {
        string sceneObjectID;
        SceneVar sceneVar;
    }

    public static class SceneEventManager
    {
        private static Dictionary<string, Action<SceneVar>> eventDico = new();


        public static void StartListening(string keyEvent, Action<SceneVar> listener)
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

        public static void StopListening(string keyEvent, Action<SceneVar> listener)
        {
            if (eventDico.ContainsKey(keyEvent))
            {
                eventDico[keyEvent] -= listener;
            }
        }

        public static void TriggerEvent(string keyEvent, SceneVar param)
        {
            if (eventDico.TryGetValue(keyEvent, out Action<SceneVar> thisEvent))
            {
                thisEvent.Invoke(param);
            }
        }
    }
}
