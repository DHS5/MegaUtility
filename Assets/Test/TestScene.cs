using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Events;
using Dhs5.Utility.SceneCreation;
using Dhs5.Utility.Events;
using System;

namespace Dhs5.Test
{
    public class TestScene : MonoBehaviour
    {
        public SceneVariablesSO sceneVariablesSO;
        //
        public SceneVarTween maxEnemy;
        //
        //public SceneEvent sceneEvent;
        //
        public UnityEvent event1;

        public AdvancedUnityEvent advancedEvent;

        public List<SceneParameteredEvent2> advancedEvents;

        public SceneParameteredEvent sceneParamEvent;

        private void OnValidate()
        {
            maxEnemy.SetUp(sceneVariablesSO, SceneVarType.INT, true);
            //sceneEvent.SetUp(sceneVariablesSO);
            sceneParamEvent.SetUp(sceneVariablesSO);
            advancedEvents.SetUp(sceneVariablesSO);
        }

        public void Trigger()
        {
            sceneParamEvent.Trigger();
        }

        [Preserve]
        public void Test()
        {
            Debug.Log(maxEnemy.Value);
        }

        public void Test(object obj)
        {
            
        }

        [Preserve]
        public void Test(int test)
        {
            Debug.Log("it works : " + test);
        }
        [Preserve]
        public void Test2(bool testBool, string testString)
        {
            Debug.Log(testString + " is " + testBool);
        }
        [Preserve]
        public void Test3(bool testBool, bool testString)
        {
            Debug.Log(testString + " is " + testBool);
        }
        [Preserve]
        public void TestUltime(string s1, bool b1, string s2, int i1, float f1)
        {
            Debug.Log(s1 + " " + b1 + " " + s2 + " " + i1 + " / " + f1 + " c'est incroyable");
        }
    }
}
