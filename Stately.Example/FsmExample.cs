namespace Stately.Example
{
    public class FsmExample
    {
        private IFsm<ConcreteClass, ExampleTransitions> _stateMachine;

        public enum ExampleTransitions
        {
            ReadyToAction,
            Stop
        }

        public FsmExample()
        {
            //Create the FSM and some Data class to be refrenced in states
            _stateMachine = new Fsm<ConcreteClass, ExampleTransitions>();
            var data = new StateData();

            //Add States
            _stateMachine.AddState(new IdleState(data));
            _stateMachine.AddState(new ActionState(data));

            //AddTransitions
            _stateMachine.AddTransition<IdleState, ActionState>(ExampleTransitions.ReadyToAction);
            _stateMachine.AddTransition<ActionState, IdleState>(ExampleTransitions.Stop);

            //Start the Machine
            _stateMachine.SetInitialState<IdleState>();
            _stateMachine.Start();
        }


        public abstract class ConcreteClass : State
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

        public class ReadyToAction : Transition
        {
        }

        public class Stop : Transition
        {
        }

        public class StateData
        {
            public int SomeArbitraryIntValue;
        }
    }
}