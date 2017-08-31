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
            StateMachine.AddTransition<StateOne, StateTwo>(TransitionActions.TriggerOne, new TransitionOne());
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

        [Test]
        public void HasTransitionReturnsTrueIfATransitionWasAddedBetweenTwoStates()
        {
            //Arrange
            AddStateAt(0);
            AddStateAt(1);
            AddSimpleTransition();
            
            //Act
            var hasTransitionReturnsTrue = StateMachine.HasTransition<StateOne, StateTwo>();
            
            //Assert
            Assert.That(hasTransitionReturnsTrue, Is.True);
            
        }
        
        [Test]
        public void HasTransitionReturnsFalseIfATransitionWasNotAddedBetweenTwoStates()
        {
            //Arrange
            AddStateAt(0);
            AddStateAt(1);
            
            //Act
            var hasTransitionReturnsFalse = !StateMachine.HasTransition<StateOne, StateTwo>();
            
            //Assert
            Assert.That(hasTransitionReturnsFalse, Is.True);
            
        }

        [Test]
        public void DoesAddingATransitionUsingAnActionCreateANormalTransition()
        {
            //Arrange
            AddStateAt(0);
            AddStateAt(1);
            var isChanged = false;
            
            //Act
            StateMachine.AddTransition<StateOne, StateTwo>(TransitionActions.TriggerOne, () => isChanged = true);
            StateMachine.SetInitialState<StateOne>();
            StateMachine.Start();
            StateMachine.TriggerTransition(TransitionActions.TriggerOne);
            
            //Assert
            Assert.That(isChanged, Is.True);
        }
        
        [Test]
        public void DoesAddingATransitionWithNoTransactionCreateANormalEmptyTransition()
        {
            //Arrange
            AddStateAt(0);
            AddStateAt(1);
            var isChanged = false;
            
            //Act
            StateMachine.AddTransition<StateOne, StateTwo>(TransitionActions.TriggerOne);
            var hasTransition = StateMachine.HasTransition<StateOne, StateTwo>();
            
            //Assert
            Assert.That(hasTransition, Is.True);
        }
    }
}