using NUnit.Framework;
using Stately.Exceptions;
using Stately.Tests.MockData;

namespace Stately.Tests.FsmTests
{
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
            StateMachine.SetInitialState<StateOne>();

            //Act
            StateMachine.Start();

            //Assert
            Assert.That(StateMachine.IsStarted, Is.True);
        }

        [Test]
        public void IsCurrentStateNullBeforeStartIsCalled()
        {
            //Assert
            Assert.That(StateMachine.CurrentState, Is.Null);
        }

        [Test]
        public void DoesCallingStartWithoutAnInitialStateThrowAnInitalStateNullException()
        {
            //Assert
            Assert.Throws<InitalStateNullException>(StateMachine.Start);
        }

        [Test]
        public void DoesCallingStartWithAnIntialStateMakeThatStateTheCurrentState()
        {
            //Arrange
            var initialStateType = typeof (StateOne);


            //Act
            StateMachine.SetInitialState<StateOne>();
            StateMachine.Start();
            var currentStateType = StateMachine.CurrentState.GetType();

            //Assert
            Assert.That(currentStateType, Is.EqualTo(initialStateType));
        }
    }
}