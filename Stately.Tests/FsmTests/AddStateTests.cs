﻿using NUnit.Framework;
using Stately.Exceptions;
using Stately.Tests.MockData;

namespace Stately.Tests.FsmTests
{
    public class AddStateTests : FsmTests
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
            Assert.That(StateMachine.StateCount, Is.EqualTo(amountOfStatesAdded));
        }

        [Test]
        public void DoesAddAGlobalStateIncreaseTheStateCount()
        {
            //Arrange
            var amountOfStates = StateMachine.StateCount;
            

            //Act
            CreateGlobalState();

            //Assert
            Assert.That(amountOfStates, Is.EqualTo(StateMachine.StateCount-1));
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

        private void CreateGlobalState()
        {
            StateMachine.AddGlobalState(new StateOne());
        }
    }
}