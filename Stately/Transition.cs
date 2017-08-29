using System;

namespace Stately
{
    public class Transition : ITransition
    {
        public virtual void Trigger()
        {
        }
    }

    public class ActionTransition : Transition
    {
        public Action _triggerAction;
        
        public ActionTransition(Action triggerAction)
        {
            _triggerAction = triggerAction;
        }

        public override void Trigger()
        {
            base.Trigger();
            _triggerAction?.Invoke();
        }
    }
}