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
            Debug.Log("it works : " + test);
        }

        public void Test2(bool testBool, string testString)
        {
            Debug.Log(testString + " is " + testBool);
        }
        public void Test3(bool testBool, bool testString)
        {
            Debug.Log(testString + " is " + testBool);
        }

        public void TestUltime(string s1, bool b1, string s2, int i1, float f1)
        {
            Debug.Log(s1 + " " + b1 + " " + s2 + " " + i1 + " / " + f1 + " c'est incroyable");
        }
    }
}
