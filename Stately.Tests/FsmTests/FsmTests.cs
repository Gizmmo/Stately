using NUnit.Framework;
using Stately.Tests.MockData;

namespace Stately.Tests.FsmTests
{
    [TestFixture]
    [Category("FsmCore")]
    public class FsmTests
    {
        protected Fsm<ConcreteState> StateMachine;

        [SetUp]
        public virtual void Init()
        {
            StateMachine = new Fsm<ConcreteState>();
        }
        
        protected void AddStates(int stateCount)
        {
            //Act
            for (var i = 0; i < stateCount; i++)
                AddStateAt(i);
        }

        protected void RemoveStates(int stateCount)
        {
            //Act
            for (var i = 0; i < stateCount; i++)
                RemoveStateAt(i);
        }

        /// <summary>
        /// Adds the state to the state machine based on the number passed
        /// </summary>
        /// <param name="state">Will add the CurrentState+1 to the state machine</param>
        protected void AddStateAt(int state)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (state)
            {
                case 0:
                    StateMachine.AddState(new StateOne());
                    break;
                case 1:
                    StateMachine.AddState(new StateTwo());
                    break;
                case 2:
                    StateMachine.AddState(new StateThree());
                    break;
                case 3:
                    StateMachine.AddState(new StateFour());
                    break;
                case 4:
                    StateMachine.AddState(new StateFive());
                    break;
            }
        }

        /// <summary>
        /// removes the state from the state machine based on the number passed
        /// </summary>
        /// <param name="state">Will remove the CurrentState+1 from the state machine</param>
        protected void RemoveStateAt(int state)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (state)
            {
                case 0:
                    StateMachine.RemoveState<StateOne>();
                    break;
                case 1:
                    StateMachine.RemoveState<StateTwo>();
                    break;
                case 2:
                    StateMachine.RemoveState<StateThree>();
                    break;
                case 3:
                    StateMachine.RemoveState<StateFour>();
                    break;
                case 4:
                    StateMachine.RemoveState<StateFive>();
                    break;
            }
        }

        /// <summary>
        /// Adds StateOne to the state machine
        /// </summary>
        protected void AddSimpleState()
        {
            StateMachine.AddState(new StateOne());
        }
    }
}