using NUnit.Framework;
using Stately.Exceptions;
using Stately.Tests.MockData;

namespace Stately.Tests.FsmTests
{
    public class AddTransitionTests : FsmTests
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
}