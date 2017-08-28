using NUnit.Framework;
using Stately.Exceptions;
using Stately.Tests.MockData;

namespace Stately.Tests.FsmTests
{
    public abstract class TransitionTests : FsmTests
    {

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
                StateMachine.AddTransition<StateOne, StateTwo>(new TransitionOne());
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
                var entryInSecondState = ((StateTwo) StateMachine.State).IsUniqueEntryCalled;

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
                Assert.That(StateMachine.State.AmountOfEntryCalls, Is.EqualTo(1));
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
                var firstState = StateMachine.State;

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
                var firstState = StateMachine.State;

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
                var firstState = StateMachine.State;

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

        public class RemoveTransitionTests : TransitionTests
        {
            [Test]
            public void DoesRemovingATransitionReturnTrueIfRemovedSuccessfully()
            {
                //Arrange
                AddStateAt(0);
                AddStateAt(1);

                StateMachine.AddTransition<StateOne, StateTwo>(new TransitionOne());

                //Act
                var returnValueOfRemove = StateMachine.RemoveTransition<TransitionOne, StateOne>();

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
                var returnValueOfRemove = StateMachine.RemoveTransition<TransitionOne, StateOne>();

                //Assert
                Assert.That(returnValueOfRemove, Is.False);
            }

            [Test]
            public void DoesRemovingATransitionThenTriggeringItThrowATransitionNotFoundException()
            {
                //Arrange
                AddStateAt(0);
                AddStateAt(1);
                StateMachine.SetInitialState<StateOne>();
                StateMachine.Start();
                StateMachine.AddTransition<StateOne, StateTwo>(new TransitionOne());

                //Act
                StateMachine.RemoveTransition<TransitionOne, StateOne>();


                //Assert
                Assert.Throws<TransitionNotFoundException>(StateMachine.TriggerTransition<TransitionOne>);
            }
        }
    }
}