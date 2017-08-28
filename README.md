# Stately

Stately is a simple finite state machine created for use in Unity, but uses to UnityEngine components.  Below is some simple example code on how to set up and use an FSM.

```
namespace Stately.Example
{
    public class FsmExample
    {
        private IFsm<ConcreteClass> _stateMachine;

        public FsmExample()
        {
            //Create the FSM and some Data class to be refrenced in states
            _stateMachine = new Fsm<ConcreteClass>();
            var data = new StateData();

            //Add States
            _stateMachine.AddState(new IdleState(data));
            _stateMachine.AddState(new ActionState(data));

            //AddTransitions
            _stateMachine.AddTransition<IdleState, ActionState>(new ReadyToAction());
            _stateMachine.AddTransition<ActionState, IdleState>(new Stop());

            //Start the Machine
            _stateMachine.SetInitialState<IdleState>();
            _stateMachine.Start();
        }


        public abstract class ConcreteClass : FsmState
        {
            protected StateData Data;

            protected ConcreteClass(StateData data)
            {
                Data = data;
            }

            public virtual void DoAction()
            {
            }

            public virtual void ReadyAction()
            {
            }
        }

        public class IdleState : ConcreteClass
        {
            public IdleState(StateData data) : base(data)
            {
            }
        }

        public class ActionState : ConcreteClass
        {
            public override void DoAction()
            {
                base.DoAction();
                Data.SomeArbitraryIntValue++;
            }

            public ActionState(StateData data) : base(data)
            {
            }
        }

        public class ReadyToAction : FsmTransition
        {
        }

        public class Stop : FsmTransition
        {
        }

        public class StateData
        {
            public int SomeArbitraryIntValue;
        }
    }
}
```
