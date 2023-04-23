using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Dhs5.Utility.SceneCreation;
using System;

namespace Dhs5.Test
{
    public class TestScene : MonoBehaviour
    {
        //public SceneVariablesSO sceneVariablesSO;
        //
        //public SceneVarTween maxEnemy;
        //
        //public SceneEvent sceneEvent;
        //
        //public UnityEvent event1;

        public AdvancedUnityEvent advancedEvent;

        //private void OnValidate()
        //{
        //    maxEnemy.SetUp(sceneVariablesSO, SceneVarType.INT);
        //    sceneEvent.SetUp(sceneVariablesSO);
        //}

        public void Trigger()
        {
            advancedEvent.Trigger();
        }

        public void Test()
        {
            Debug.Log("It works");
        }

        public void Test(object obj)
        {

        }

        public void Test(int test)
        {

        }

        public void Test(bool test)
        {

        }
    }
}
