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

        public SceneEvent sceneEvent;

        private void OnValidate()
        {
            maxEnemy.sceneVariablesSO = sceneVariablesSO;
            sceneEvent.SetUp(sceneVariablesSO);
        }

        private void Test()
        {
            Debug.Log(maxEnemy.IntValue);
        }

        public void Test2(object obj)
        {

        }
    }
}
