using NUnit.Framework;
using Stately.Exceptions;
using Stately.Tests.MockData;

namespace Stately.Tests.FsmTests
{
    public class RemoveStateTests : FsmTests
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
            Assert.That(StateMachine.StateCount, Is.EqualTo(expectedRemainingStatesCount));
        }
        
        [Test]
        public void DoesRemovingAStateThatIsInTheFsmReturnTrue()
        {
            //Arrange
            StateMachine.AddState(new StateOne());
            //Act
            var stateWasRemoved = StateMachine.RemoveState<StateOne>();
            //Assert
            Assert.That(stateWasRemoved, Is.True);
        }

        [Test]
        public void DoesRemovingAStateThatIsNotInTheFsmReturnFalse()
        {
            //Arrange

            //Act
            var stateWasRemoved = StateMachine.RemoveState<StateOne>();
            //Assert
            Assert.That(stateWasRemoved, Is.False);
        }

        [Test]
        public void DoesRemovingTheCurrentStateThrowARemoveCurrentStateException()
        {
            //Arrange
            AddStateAt(0);
            StateMachine.SetInitialState<StateOne>();

            //Act
            StateMachine.Start();

            //Assert
            Assert.Throws<RemoveCurrentStateException>(() => StateMachine.RemoveState<StateOne>());
        }
    }
}