using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace Dhs5.Utility.SceneCreation
{
    [Serializable]
    public class AdvancedUnityEvent
    {
        public Component component;
        public BaseEventAction action;

        public string myString;

        #region Custom Property Drawer variables
        [SerializeField] private int metadataToken;
        #endregion

        public void Trigger()
        {
            if (action == null)
            {
                Debug.LogError("Action is null");
                return;
            }
            action.Invoke();
        }
    }

    [Serializable]
    public class BaseEventAction
    {
        protected object[] arguments;

        //public BaseEventAction(object[] args)
        //{
        //    arguments = args;
        //}
        public BaseEventAction() { }

        //public abstract void Invoke();
        public virtual void Invoke() { }
    }

    [Serializable]
    public class EventAction : BaseEventAction
    {
        private Action action;

        //public EventAction(object[] args, Action _action) : base(args)
        //{
        //    action = _action;
        //}
        public EventAction(Action _action)
        {
            action = _action;
        }

        public override void Invoke()
        {
            action?.Invoke();
        }
    }
    [Serializable]
    public class EventAction<T1> : BaseEventAction
    {
        private Action<T1> action;

        //public EventAction(object[] args, Action<T1> _action) : base(args)
        //{
        //    action = _action;
        //}
        T1 arg0;
        public EventAction(Action<T1> _action, T1 _arg0)
        {
            action = _action;
            arg0 = _arg0;
        }

        public override void Invoke()
        {
            action?.Invoke(arg0);
        }
    }
    public class EventAction<T1, T2> : BaseEventAction
    {
        private Action<T1, T2> action;

        //public EventAction(object[] args, Action<T1, T2> _action) : base(args)
        //{
        //    action = _action;
        //}
        T1 arg0;
        T2 arg1;
        public EventAction(Action<T1, T2> _action, T1 _arg0, T2 _arg1)
        {
            action = _action;
            arg0 = _arg0;
            arg1 = _arg1;
        }

        public override void Invoke()
        {
            action?.Invoke(arg0, arg1);
        }
    }
}
