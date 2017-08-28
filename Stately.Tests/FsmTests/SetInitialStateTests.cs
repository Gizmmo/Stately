using NUnit.Framework;
using Stately.Exceptions;
using Stately.Tests.MockData;

namespace Stately.Tests.FsmTests
{
    public class SetInitialStateTests : FsmTests
    {
        [Test]
        public void DoesInitalStateSetTheInitialState()
        {
            //Arrange
            var initalStateType = typeof (StateOne);
            AddStateAt(0);

            //Act
            StateMachine.SetInitialState<StateOne>();

            //Assert
            Assert.That(StateMachine.InitialState, Is.EqualTo(initalStateType));
        }

        [Test]
        public void DoesSettingTheInitalStateWhenThatStateIsNotInTheFsmThrowStateNotFoundException()
        {
            //Assert
            Assert.Throws<StateNotFoundException>(StateMachine.SetInitialState<StateOne>);
        }
    }
}