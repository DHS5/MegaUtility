using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dhs5.Utility.SceneCreation;

namespace Dhs5.Test
{
    public class TestScene : MonoBehaviour
    {
        public SceneVariablesSO sceneVariablesSO;

        public SceneVarTween maxEnemy;

        private void OnValidate()
        {
            maxEnemy.sceneVariablesSO = sceneVariablesSO;
        }

        private void Test()
        {
            Debug.Log(maxEnemy.IntValue);
        }
    }
}
