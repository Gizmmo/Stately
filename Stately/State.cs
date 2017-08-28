using System;

namespace Stately
{
    public abstract class State : IState
    {
        public virtual void OnEntry()
        {
        }

        public virtual void OnExit()
        {
        }
    }
}