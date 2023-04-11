using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Dhs5.Utility.SceneCreation
{
    [RequireComponent(typeof(SceneObject))]
    public class SceneTrigger : MonoBehaviour
    {
        public SceneObject sceneObject;

        [Header("Triggers")]
        public List<SceneAction> triggers;

        public void Trigger()
        {
            foreach (SceneAction action in triggers)
                action.Trigger();
        }
    }
}
