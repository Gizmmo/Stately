using System;

namespace Stately
{
    public abstract class FsmState : IState
    {

        private Action<Type> _transitionAction;

        public virtual void OnEntry()
        {
        }

        public virtual void OnExit()
        {
        }

        public void SetUpTransition(Action<Type> transitionMethod)
        {
            _transitionAction = transitionMethod;
        }


        protected void TriggerTransition<T>() where T : ITransition, new()
        {
            _transitionAction(typeof(T));
        }
    }
}