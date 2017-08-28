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