using NUnit.Framework;
using Stately.Exceptions;
using Stately.Tests.MockData;

namespace Stately.Tests.FsmTests
{
    public class TriggerTransitionTests : FsmTests
    {
        [Test]
        public void DoesTriggerTransitionCallTheTriggerMethodOfThePassedTrigger()
        {
            //Arrange
            AddStateAt(0);
            AddStateAt(1);
            var stateTransition = new TransitionOne();
            StateMachine.AddTransition<StateOne, StateTwo>(stateTransition);
            StateMachine.SetInitialState<StateOne>();
            StateMachine.Start();

            //Act
            StateMachine.TriggerTransition<TransitionOne>();

            //Assert
            Assert.That(stateTransition.IsTriggered, Is.True);
        }

        [Test]
        public void DoesTriggerTransitionBeforeRunningStartReturnAStateMachineNotStartedException()
        {
            //Arrange
            AddStateAt(0);
            AddStateAt(1);

            StateMachine.AddTransition<StateOne, StateTwo>(new TransitionOne());
            StateMachine.SetInitialState<StateOne>();

            //Act

            //Assert
            Assert.Throws<StateMachineNotStartedException>(StateMachine.TriggerTransition<TransitionOne>);
        }

        [Test]
        public void DoesTriggeringATransitionCauseTheNextStatesOnEntryToBeCalled()
        {
            //Arrange
            AddStateAt(0);
            AddStateAt(1);

            StateMachine.AddTransition<StateOne, StateTwo>(new TransitionOne());
            StateMachine.SetInitialState<StateOne>();
            StateMachine.Start();

            //Act
            StateMachine.TriggerTransition<TransitionOne>();
            var entryInSecondState = ((StateTwo) StateMachine.CurrentState).IsUniqueEntryCalled;

            //Assert
            Assert.That(entryInSecondState, Is.True);
        }

        [Test]
        public void DoesTriggerTransitionOnlyCallTheOnEntryOnceForTheStateBeingMovedTo()
        {
            //Arrange
            AddStateAt(0);
            AddStateAt(1);

            StateMachine.AddTransition<StateOne, StateTwo>(new TransitionOne());
            StateMachine.SetInitialState<StateOne>();
            StateMachine.Start();

            //Act
            StateMachine.TriggerTransition<TransitionOne>();

            //Assert
            Assert.That(StateMachine.CurrentState.AmountOfEntryCalls, Is.EqualTo(1));
        }

        [Test]
        public void DoesTriggerTransitionOnlyCallTheOnEntryOnceForTheStateBeingMovedFrom()
        {
            //Arrange
            AddStateAt(0);
            AddStateAt(1);

            StateMachine.AddTransition<StateOne, StateTwo>(new TransitionOne());
            StateMachine.SetInitialState<StateOne>();
            StateMachine.Start();
            var firstState = StateMachine.CurrentState;

            //Act
            StateMachine.TriggerTransition<TransitionOne>();

            //Assert
            Assert.That(firstState.AmountOfEntryCalls, Is.EqualTo(1));
        }

        [Test]
        public void DoesTriggerTransitionCallTheOnExitMethodOfTheStateFromStateWhenLeaving()
        {
            //Arrange
            AddStateAt(0);
            AddStateAt(1);

            StateMachine.AddTransition<StateOne, StateTwo>(new TransitionOne());
            StateMachine.SetInitialState<StateOne>();
            StateMachine.Start();
            var firstState = StateMachine.CurrentState;

            //Act
            StateMachine.TriggerTransition<TransitionOne>();

            //Assert
            Assert.That(firstState.IsExitCalled, Is.True);
        }

        [Test]
        public void DoesTriggerTransitionOnCallTheOnExitOnceForTheStateBeingLeft()
        {
            //Arrange
            AddStateAt(0);
            AddStateAt(1);

            StateMachine.AddTransition<StateOne, StateTwo>(new TransitionOne());
            StateMachine.SetInitialState<StateOne>();
            StateMachine.Start();
            var firstState = StateMachine.CurrentState;

            //Act
            StateMachine.TriggerTransition<TransitionOne>();

            //Assert
            Assert.That(firstState.AmountOfExitCalls, Is.EqualTo(1));
        }

        [Test]
        public void DoesTriggerTransitionTheOnExitTriggerBeforeTheOnEntryOfTheNextState()
        {
            //Arrange
            var testObject = new EntryExitTestObject();
            StateMachine.AddState(new StateOne(testObject));
            StateMachine.AddState(new StateTwo(testObject));

            StateMachine.AddTransition<StateOne, StateTwo>(new TransitionOne());
            StateMachine.SetInitialState<StateOne>();
            StateMachine.Start();

            //Act
            StateMachine.TriggerTransition<TransitionOne>();

            //Assert
            Assert.That(testObject.ExitCalledBeforeSecondEntry, Is.True);
        }
    }
}