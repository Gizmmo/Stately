using NUnit.Framework;
using Stately.Exceptions;
using Stately.Tests.MockData;

namespace Stately.Tests.FsmTests
{
    public class RemoveTransitionTests : FsmTests
    {
        [Test]
        public void DoesRemovingATransitionReturnTrueIfRemovedSuccessfully()
        {
            //Arrange
            AddStateAt(0);
            AddStateAt(1);

            StateMachine.AddTransition<StateOne, StateTwo>(TransitionActions.TriggerOne, new TransitionOne());

            //Act
            var returnValueOfRemove = StateMachine.RemoveTransition<StateOne>(TransitionActions.TriggerOne);

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
            var returnValueOfRemove = StateMachine.RemoveTransition<StateOne>(TransitionActions.TriggerOne);

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
            StateMachine.AddTransition<StateOne, StateTwo>(TransitionActions.TriggerOne, new TransitionOne());

            //Act
            StateMachine.RemoveTransition<StateOne>(TransitionActions.TriggerOne);


            //Assert
            Assert.Throws<TransitionNotFoundException>(() => StateMachine.TriggerTransition(TransitionActions.TriggerOne));
        }

        [Test]
        public void DoesRemoveAllTransitionsRemoveAllTheTransitionsFromThePassedState()
        {
            //Arrange
            AddStateAt(0);
            AddStateAt(1);
            StateMachine.SetInitialState<StateOne>();
            StateMachine.AddTransition<StateOne, StateTwo>(TransitionActions.TriggerOne, new TransitionOne());
            StateMachine.AddTransition<StateOne, StateTwo>(TransitionActions.TriggerTwo, () => {} );
            
            //Act
            StateMachine.RemoveAllTransitions<StateOne>();
            var hasEitherTransitions = StateMachine.HasTransition<StateOne, StateTwo>();
            //Assert
            Assert.That(hasEitherTransitions, Is.False);
        } 
    }
}