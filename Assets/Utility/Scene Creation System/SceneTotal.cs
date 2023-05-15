using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dhs5.Utility.SceneCreation
{
    [System.Serializable]
    public class SceneTotal : SceneState.ISceneVarTypedSetupable
    {
        #region Operator
        public enum Operator
        {
            ADD,
            SUBTRACT,
            MULTIPLY,
            DIVIDE,
            POWER
        }
        #endregion

        [SerializeField] private SceneVariablesSO sceneVariablesSO;

        [SerializeField] private SceneVarTween varTween;

        [SerializeField] private Operator op;
        public Operator Op => op;

        [SerializeField] private SceneVarType type;

        public object Value => varTween.Value;

        public void SetUp(SceneVariablesSO sceneVariablesSO, SceneVarType type)
        {
            if (type == SceneVarType.BOOL || type == SceneVarType.EVENT)
                type = SceneVarType.INT;
            this.type = type;

            this.sceneVariablesSO = sceneVariablesSO;
            varTween.SetUp(sceneVariablesSO, this.type, true);
        }
    }
}
