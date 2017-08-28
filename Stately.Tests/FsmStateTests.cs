using NUnit.Framework;

namespace Stately.Tests
{
    [TestFixture]
    public class FsmStateTests
    {
        private Fsm<ConcreteState> _fsm;

        [SetUp]
        public void Init()
        {
            _fsm = new Fsm<ConcreteState>();
            _fsm.AddState(new StateOne());
            _fsm.AddState(new StateTwo());
            _fsm.AddTransition<StateOne, StateTwo>(new Transition());
            _fsm.SetInitialState<StateOne>();
            _fsm.Start();
        }

        [Test]
        public void DoesCallingAMethodThatCallsTriggerTransitionCauseTheFsmToChangeStates()
        {
            //Arrange
            var stateTwo = typeof(StateTwo);
            
            //Act
            _fsm.CurrentState.ChangeToTwo();
            var currentStateOfStateMachine = _fsm.CurrentState.GetType();
            
            //Assert
            Assert.That(currentStateOfStateMachine, Is.EqualTo(stateTwo));
        }

        public class ConcreteState : State
        {
            public virtual void ChangeToTwo()
            {
            }
        }

        public class StateOne : ConcreteState
        {
            public override void ChangeToTwo()
            {
                base.ChangeToTwo();
                TriggerTransition<Transition>();
            }
        }

        public class StateTwo : ConcreteState
        {
            
        }

        public class Transition : ITransition
        {
            public void Trigger()
            {
            }
        }
    }
}