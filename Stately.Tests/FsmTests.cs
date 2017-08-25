using System;
using NUnit.Framework;

namespace Stately.Tests
{
    [TestFixture]
    [Category("FsmCore")]
    public class FsmTests
    {
        private Fsm<ConcreteState> _fsm;

        [SetUp]
        public virtual void Init()
        {
            _fsm = new Fsm<ConcreteState>();
        }

        /// <summary>
        /// Adds the state to the state machine based on the number passed
        /// </summary>
        /// <param name="state">Will add the State+1 to the state machine</param>
        protected void AddStateAt(int state)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (state)
            {
                case 0:
                    _fsm.AddState(new StateOne());
                    break;
                case 1:
                    _fsm.AddState(new StateTwo());
                    break;
                case 2:
                    _fsm.AddState(new StateThree());
                    break;
                case 3:
                    _fsm.AddState(new StateFour());
                    break;
                case 4:
                    _fsm.AddState(new StateFive());
                    break;
            }
        }

        /// <summary>
        /// removes the state from the state machine based on the number passed
        /// </summary>
        /// <param name="state">Will remove the State+1 from the state machine</param>
        protected void RemoveStateAt(int state)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (state)
            {
                case 0:
                    _fsm.RemoveState<StateOne>();
                    break;
                case 1:
                    _fsm.RemoveState<StateTwo>();
                    break;
                case 2:
                    _fsm.RemoveState<StateThree>();
                    break;
                case 3:
                    _fsm.RemoveState<StateFour>();
                    break;
                case 4:
                    _fsm.RemoveState<StateFive>();
                    break;
            }
        }

        /// <summary>
        /// Adds StateOne to the state machine
        /// </summary>
        protected void AddSimpleState()
        {
            _fsm.AddState(new StateOne());
        }

        public abstract class AddAndRemovalTests : FsmTests
        {
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


            public class AddStateTests : AddAndRemovalTests
            {
                [Test]
                [TestCase(1, TestName = "OneStateAdd")]
                [TestCase(2, TestName = "TwoStateAdds")]
                [TestCase(3, TestName = "ThreeStateAdds")]
                [TestCase(4, TestName = "FourStateAdds")]
                [TestCase(5, TestName = "FiveStateAdds")]
                public void DoesAddingAGenericStateIncreasetheStateCount(int amountOfStatesAdded)
                {
                    //Arrange

                    //Act
                    AddStates(amountOfStatesAdded);

                    //Assert
                    Assert.That(_fsm.StateCount, Is.EqualTo(amountOfStatesAdded));
                }

                [Test]
                public void DoesAddingAStateThatAlreadyExistsThrowADuplicateStateException()
                {
                    //Arrange
                    AddSimpleState();

                    //Act

                    //Assert
                    Assert.Throws<DuplicateStateException>(AddSimpleState);
                }
            }

            public class RemoveStateTests : AddAndRemovalTests
            {
                [Test]
                [TestCase(1, TestName = "OneStateRemove")]
                [TestCase(2, TestName = "TwoStateRemoves")]
                [TestCase(3, TestName = "ThreeStateRemoves")]
                [TestCase(4, TestName = "FourStateRemoves")]
                [TestCase(5, TestName = "FiveStateRemoves")]
                public void DoesRemovingAStateFromFsmDecreaseTheStateCount(int stateCount)
                {
                    //Arrange
                    const int maxStates = 5;
                    AddStates(maxStates);
                    var expectedRemainingStatesCount = maxStates - stateCount;

                    //Act
                    RemoveStates(stateCount);

                    //Assert
                    Assert.That(_fsm.StateCount, Is.EqualTo(expectedRemainingStatesCount));
                }
            }

            [Test]
            public void DoesRemovingAStateThatIsInTheFsmReturnTrue()
            {
                //Arrange
                _fsm.AddState(new StateOne());
                //Act
                var stateWasRemoved = _fsm.RemoveState<StateOne>();
                //Assert
                Assert.That(stateWasRemoved, Is.True);
            }

            [Test]
            public void DoesRemovingAStateThatIsNotInTheFsmReturnFalse()
            {
                //Arrange

                //Act
                var stateWasRemoved = _fsm.RemoveState<StateOne>();
                //Assert
                Assert.That(stateWasRemoved, Is.False);
            }

            [Test]
            public void DoesRemovingTheCurrentStateThrowARemoveCurrentStateException()
            {
                //Arrange
                AddStateAt(0);
                _fsm.SetInitialState<StateOne>();

                //Act
                _fsm.Start();

                //Assert
                Assert.Throws<RemoveCurrentStateException>(RemoveState);
            }

            private void RemoveState()
            {
                _fsm.RemoveState<StateOne>();
            }
        }

        public class InitalizeTests : FsmTests
        {
            [Test]
            public void DoesInitalStateSetTheInitialState()
            {
                //Arrange
                var initalStateType = typeof (StateOne);
                AddStateAt(0);

                //Act
                _fsm.SetInitialState<StateOne>();

                //Assert
                Assert.That(_fsm.InitialState, Is.EqualTo(initalStateType));
            }

            [Test]
            public void DoesSettingTheInitalStateWhenThatStateIsNotInTheFsmThrowStateNotFoundException()
            {
                //Assert
                Assert.Throws<StateNotFoundException>(_fsm.SetInitialState<StateOne>);
            }
        }

        public class StartTests : FsmTests
        {
            public override void Init()
            {
                base.Init();
                AddStateAt(0);
            }

            [Test]
            public void DoesIsStartedTurnToTrueWhenTheFsmIsStarted()
            {
                //Arrange
                _fsm.SetInitialState<StateOne>();

                //Act
                _fsm.Start();

                //Assert
                Assert.That(_fsm.IsStarted, Is.True);
            }

            [Test]
            public void IsCurrentStateNullBeforeStartIsCalled()
            {
                //Assert
                Assert.That(_fsm.State, Is.Null);
            }

            [Test]
            public void DoesCallingStartWithoutAnInitialStateThrowAnInitalStateNullException()
            {
                //Assert
                Assert.Throws<InitalStateNullException>(_fsm.Start);
            }

            [Test]
            public void DoesCallingStartWithAnIntialStateMakeThatStateTheCurrentState()
            {
                //Arrange
                var initialStateType = typeof (StateOne);


                //Act
                _fsm.SetInitialState<StateOne>();
                _fsm.Start();
                var currentStateType = _fsm.State.GetType();

                //Assert
                Assert.That(currentStateType, Is.EqualTo(initialStateType));
            }
        }

        public class CurrentStateTests : FsmTests
        {
            public override void Init()
            {
                base.Init();
                AddStateAt(0);
                _fsm.SetInitialState<StateOne>();
            }

            [Test]
            public void DoesCallingACurrentStateMethodCallTheSharedMethod()
            {
                //Arrange
                const int returnNumber = 0;

                //Act
                _fsm.Start();
                var sharedMethodWasCalled = returnNumber == _fsm.State.ReturnZero();

                //Assert
                Assert.That(sharedMethodWasCalled);
            }

            [Test]
            public void DoesCallingACurrentStateMethodCallTheOverriddenMethod()
            {
                //Arrange
                const int returnNumber = 1;

                //Act
                _fsm.Start();
                var overiddenMethodWasCalled = returnNumber == _fsm.State.ReturnStateNumber();

                //Assert
                Assert.That(overiddenMethodWasCalled);
            }

            [Test]
            public void DoesSettingToCurrentStateCauseOnEntryToBeCalled()
            {
                //Arrange

                //Act
                _fsm.Start();
                var entryWasCalled = _fsm.State.IsEntryCalled;

                //Assert
                Assert.That(entryWasCalled);
            }
        }

        public abstract class TransitionTests : FsmTests
        {
            protected static bool TriggerCalled;

            public override void Init()
            {
                base.Init();
                TriggerCalled = false;
            }

            public class AddTransitionTests : TransitionTests
            {
                [Test]
                public void DoesAddingATransitionBetweenTwoStatesNotReturnAnError()
                {
                    //Arrange
                    AddStateAt(0);
                    AddStateAt(1);

                    //Act

                    //Assert
                    Assert.DoesNotThrow(AddSimpleTransition);
                }

                private void AddSimpleTransition()
                {
                    _fsm.AddTransition<StateOne, StateTwo>(new StateOneToStateTwoTransition());
                }

                [Test]
                [TestCase(0, TestName = "StateFrom exists, StateTo does not")]
                [TestCase(1, TestName = "StateFrom does not exists, StateTo does")]
                [TestCase(2, TestName = "StateFrom does not exists, StateTo does not exist")]
                public void DoesAddingATransitionBetweenAStateThatExistsAndOneThatDoesNotReturnAStateNotFoundException(
                    int state)
                {
                    //Arrange
                    AddStateAt(state);

                    //Act

                    //Assert
                    Assert.Throws<StateNotFoundException>(AddSimpleTransition);
                }

                [Test]
                public void DoesAddingTheSameTransitionToTheSameStateThrowADuplicateStateTransitionException()
                {
                    //Arrange
                    AddStateAt(0);
                    AddStateAt(1);

                    //Act
                    AddSimpleTransition();

                    //Assert
                    Assert.Throws<DuplicateStateTransitionException>(AddSimpleTransition);
                }
            }

            public class TriggerTransitionTests : TransitionTests
            {
                [Test]
                public void DoesTriggerTransitionCallTheTriggerMethodOfThePassedTrigger()
                {
                    //Arrange
                    AddStateAt(0);
                    AddStateAt(1);

                    _fsm.AddTransition<StateOne, StateTwo>(new StateOneToStateTwoTransition());
                    _fsm.SetInitialState<StateOne>();
                    _fsm.Start();

                    //Act
                    _fsm.TriggerTransition<StateOneToStateTwoTransition>();

                    //Assert
                    Assert.That(TriggerCalled, Is.True);
                }

                [Test]
                public void DoesTriggerTransitionBeforeRunningStartReturnAStateMachineNotStartedException()
                {
                    //Arrange
                    AddStateAt(0);
                    AddStateAt(1);

                    _fsm.AddTransition<StateOne, StateTwo>(new StateOneToStateTwoTransition());
                    _fsm.SetInitialState<StateOne>();

                    //Act

                    //Assert
                    Assert.Throws<StateMachineNotStartedException>(_fsm.TriggerTransition<StateOneToStateTwoTransition>);
                }

                [Test]
                public void DoesTriggeringATransitionCauseTheNextStatesOnEntryToBeCalled()
                {
                    //Arrange
                    AddStateAt(0);
                    AddStateAt(1);

                    _fsm.AddTransition<StateOne, StateTwo>(new StateOneToStateTwoTransition());
                    _fsm.SetInitialState<StateOne>();
                    _fsm.Start();

                    //Act
                    _fsm.TriggerTransition<StateOneToStateTwoTransition>();
                    var entryInSecondState = ((StateTwo) _fsm.State).IsUniqueEntryCalled;

                    //Assert
                    Assert.That(entryInSecondState, Is.True);
                }

                [Test]
                public void DoesTriggerTransitionOnlyCallTheOnEntryOnceForTheStateBeingMovedTo()
                {
                    //Arrange
                    AddStateAt(0);
                    AddStateAt(1);

                    _fsm.AddTransition<StateOne, StateTwo>(new StateOneToStateTwoTransition());
                    _fsm.SetInitialState<StateOne>();
                    _fsm.Start();

                    //Act
                    _fsm.TriggerTransition<StateOneToStateTwoTransition>();

                    //Assert
                    Assert.That(_fsm.State.AmountOfEntryCalls, Is.EqualTo(1));
                }

                [Test]
                public void DoesTriggerTransitionOnlyCallTheOnEntryOnceForTheStateBeingMovedFrom()
                {
                    //Arrange
                    AddStateAt(0);
                    AddStateAt(1);

                    _fsm.AddTransition<StateOne, StateTwo>(new StateOneToStateTwoTransition());
                    _fsm.SetInitialState<StateOne>();
                    _fsm.Start();
                    var firstState = _fsm.State;

                    //Act
                    _fsm.TriggerTransition<StateOneToStateTwoTransition>();

                    //Assert
                    Assert.That(firstState.AmountOfEntryCalls, Is.EqualTo(1));
                }

                [Test]
                public void DoesTriggerTransitionCallTheOnExitMethodOfTheStateFromStateWhenLeaving()
                {
                    //Arrange
                    AddStateAt(0);
                    AddStateAt(1);

                    _fsm.AddTransition<StateOne, StateTwo>(new StateOneToStateTwoTransition());
                    _fsm.SetInitialState<StateOne>();
                    _fsm.Start();
                    var firstState = _fsm.State;

                    //Act
                    _fsm.TriggerTransition<StateOneToStateTwoTransition>();

                    //Assert
                    Assert.That(firstState.IsExitCalled, Is.True);
                }

                [Test]
                public void DoesTriggerTransitionOnCallTheOnExitOnceForTheStateBeingLeft()
                {
                    //Arrange
                    AddStateAt(0);
                    AddStateAt(1);

                    _fsm.AddTransition<StateOne, StateTwo>(new StateOneToStateTwoTransition());
                    _fsm.SetInitialState<StateOne>();
                    _fsm.Start();
                    var firstState = _fsm.State;

                    //Act
                    _fsm.TriggerTransition<StateOneToStateTwoTransition>();

                    //Assert
                    Assert.That(firstState.AmountOfExitCalls, Is.EqualTo(1));
                }

                [Test]
                public void DoesTriggerTransitionTheOnExitTriggerBeforeTheOnEntryOfTheNextState()
                {
                    //Arrange
                    var testObject = new EntryExitTestObject();
                    _fsm.AddState(new StateOne(testObject));
                    _fsm.AddState(new StateTwo(testObject));

                    _fsm.AddTransition<StateOne, StateTwo>(new StateOneToStateTwoTransition());
                    _fsm.SetInitialState<StateOne>();
                    _fsm.Start();

                    //Act
                    _fsm.TriggerTransition<StateOneToStateTwoTransition>();

                    //Assert
                    Assert.That(testObject.ExitCalledBeforeSecondEntry, Is.True);
                }
            }

            public class RemoveTransitionTests : TransitionTests
            {
                [Test]
                public void DoesRemovingATransitionReturnTrueIfRemovedSuccessfully()
                {
                    //Arrange
                    AddStateAt(0);
                    AddStateAt(1);

                    _fsm.AddTransition<StateOne, StateTwo>(new StateOneToStateTwoTransition());

                    //Act
                    var returnValueOfRemove = _fsm.RemoveTransition<StateOneToStateTwoTransition, StateOne>();

                    //Assert
                    Assert.That(returnValueOfRemove, Is.True);
                }

                [Test]
                public void DoesRemovingATransitionReturnFalseIfNotRemovedSuccessfully()
                {
                    //Arrange
                    AddStateAt(0);
                    AddStateAt(1);

                    //Act
                    var returnValueOfRemove = _fsm.RemoveTransition<StateOneToStateTwoTransition, StateOne>();

                    //Assert
                    Assert.That(returnValueOfRemove, Is.False);
                }

                [Test]
                public void DoesRemovingATransitionThenTriggeringItThrowATransitionNotFoundException()
                {
                    //Arrange
                    AddStateAt(0);
                    AddStateAt(1);
                    _fsm.SetInitialState<StateOne>();
                    _fsm.Start();
                    _fsm.AddTransition<StateOne, StateTwo>(new StateOneToStateTwoTransition());

                    //Act
                    _fsm.RemoveTransition<StateOneToStateTwoTransition, StateOne>();


                    //Assert
                    Assert.Throws<TransitionNotFoundException>(_fsm.TriggerTransition<StateOneToStateTwoTransition>);
                }
            }


            public class StateOneToStateTwoTransition : ITransition
            {
                public void Trigger()
                {
                    TriggerCalled = true;
                }
            }
        }


        public abstract class ConcreteState : IState
        {
            public bool IsEntryCalled { get; private set; }
            public bool IsExitCalled { get; private set; }
            public int AmountOfEntryCalls { get; private set; }
            public int AmountOfExitCalls { get; private set; }

            private readonly EntryExitTestObject _testObject;

            protected ConcreteState()
            {
            }

            protected ConcreteState(EntryExitTestObject testObject)
            {
                _testObject = testObject;
            }

            public int ReturnZero()
            {
                return 0;
            }

            public abstract int ReturnStateNumber();

            public virtual void OnEntry()
            {
                AmountOfEntryCalls++;
                IsEntryCalled = true;
                _testObject?.OnEntry();
            }

            public virtual void OnExit()
            {
                AmountOfExitCalls++;
                IsExitCalled = true;
                _testObject?.OnExit();
            }

            public void SetUpTransition(Action<Type> transitionMethod)
            {
            }
        }

        public class StateOne : ConcreteState
        {
            public override int ReturnStateNumber()
            {
                return 1;
            }

            public StateOne()
            {
            }

            public StateOne(EntryExitTestObject testObject) : base(testObject)
            {
            }
        }

        public class StateTwo : ConcreteState
        {
            public bool IsUniqueEntryCalled { get; private set; }

            public override int ReturnStateNumber()
            {
                return 2;
            }

            public override void OnEntry()
            {
                base.OnEntry();
                IsUniqueEntryCalled = true;
            }

            public StateTwo(EntryExitTestObject testObject) : base(testObject)
            {
            }

            public StateTwo()
            {
            }
        }

        public class StateThree : ConcreteState
        {
            public override int ReturnStateNumber()
            {
                return 3;
            }

            public StateThree(EntryExitTestObject testObject) : base(testObject)
            {
            }

            public StateThree()
            {
            }
        }

        public class StateFour : ConcreteState
        {
            public override int ReturnStateNumber()
            {
                return 4;
            }

            public StateFour(EntryExitTestObject testObject) : base(testObject)
            {
            }

            public StateFour()
            {
            }
        }

        public class StateFive : ConcreteState
        {
            public override int ReturnStateNumber()
            {
                return 5;
            }

            public StateFive(EntryExitTestObject testObject) : base(testObject)
            {
            }

            public StateFive()
            {
            }
        }

        public class EntryExitTestObject
        {
            private int _amountOfEntries;

            public bool ExitCalledBeforeSecondEntry { get; private set; }

            public void OnEntry()
            {
                _amountOfEntries++;
            }

            public void OnExit()
            {
                if (_amountOfEntries < 2)
                    ExitCalledBeforeSecondEntry = true;
            }
        }
    }
}