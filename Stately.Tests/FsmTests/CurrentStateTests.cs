using NUnit.Framework;
using Stately.Tests.MockData;

namespace Stately.Tests.FsmTests
{
    public class CurrentStateTests : FsmTests
    {
        public override void Init()
        {
            base.Init();
            AddStateAt(0);
            StateMachine.SetInitialState<StateOne>();
        }

        [Test]
        public void DoesCallingACurrentStateMethodCallTheSharedMethod()
        {
            //Arrange
            const int returnNumber = 0;

            //Act
            StateMachine.Start();
            var sharedMethodWasCalled = returnNumber == StateMachine.State.ReturnZero();

            //Assert
            Assert.That(sharedMethodWasCalled);
        }

        [Test]
        public void DoesCallingACurrentStateMethodCallTheOverriddenMethod()
        {
            //Arrange
            const int returnNumber = 1;

            //Act
            StateMachine.Start();
            var overiddenMethodWasCalled = returnNumber == StateMachine.State.ReturnStateNumber();

            //Assert
            Assert.That(overiddenMethodWasCalled);
        }

        [Test]
        public void DoesSettingToCurrentStateCauseOnEntryToBeCalled()
        {
            //Arrange

            //Act
            StateMachine.Start();
            var entryWasCalled = StateMachine.State.IsEntryCalled;

            //Assert
            Assert.That(entryWasCalled);
        }
    }
}