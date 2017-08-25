using System;
using NUnit.Framework;

namespace Stately.Tests
{
    [TestFixture]
    [Category("FsmStateContainer")]
    public class StateContainerTests
    {
        private StateContainer _container;
        private static bool _wasTriggered;

        [SetUp]
        public virtual void Init()
        {
            _wasTriggered = false;
        }

        protected void AddTransition(int transitionNum)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (transitionNum)
            {
                case 0:
                    _container.AddTransition(new TransitionOne(), typeof (FsmTests.StateTwo));
                    break;
                case 1:
                    _container.AddTransition(new TransitionTwo(), typeof (FsmTests.StateTwo));
                    break;
                case 2:
                    _container.AddTransition(new TransitionThree(), typeof (FsmTests.StateTwo));
                    break;
            }
        }

        protected void RemoveTransition(int transitionNum)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (transitionNum)
            {
                case 0:
                    _container.RemoveTransition<TransitionOne>();
                    break;
                case 1:
                    _container.RemoveTransition<TransitionTwo>();
                    break;
                case 2:
                    _container.RemoveTransition<TransitionThree>();
                    break;
            }
        }

        public class InitalizationTests : StateContainerTests
        {
            [Test]
            public void DoesInitalizingAStateContainerWithAStateMakeTheStatePropertyThatState()
            {
                //Arrange
                var stateType = typeof (TestState);
                //Act
                _container = new StateContainer(new TestState());
                var containerStateType = _container.State.GetType();

                //Assert
                Assert.That(containerStateType, Is.EqualTo(stateType));
            }
        }

        public class TransitionTests : StateContainerTests
        {
            public override void Init()
            {
                base.Init();
                _container = new StateContainer(new TestState());
            }

            [Test]
            [TestCase(0, TestName = "ZeroTransitionAdd")]
            [TestCase(1, TestName = "OneTransitionAdd")]
            [TestCase(2, TestName = "TwoTransitionAdd")]
            [TestCase(3, TestName = "ThreeTransitionAdd")]
            public void DoesAddTransitionCauseTheTransitionCountToIncreaseByOne(int amountOfTransitionsToAdd)
            {
                //Arrange

                //Act
                for (var i = 0; i < amountOfTransitionsToAdd; i++)
                    AddTransition(i);

                //Assert
                Assert.That(_container.TransitionCount, Is.EqualTo(amountOfTransitionsToAdd));
            }

            [Test]
            public void DoesAddingTheSameTransitionCauseADuplicateStateTransitionException()
            {
                //Arrange
                AddSimpleTransition();
                //Act

                //Assert
                Assert.Throws<DuplicateStateTransitionException>(AddSimpleTransition);
            }

            [Test]
            public void DoesGetTransitionReturnTheExpectedTransitionInTheContainer()
            {
                //Arrange
                var originalTransition = new TransitionOne();
                _container.AddTransition(originalTransition, typeof (FsmTests.StateTwo));

                //Act
                var returnedTransition = _container.GetTransition<TransitionOne>().Transition;

                //Assert
                Assert.That(returnedTransition, Is.EqualTo(originalTransition));
            }

            [Test]
            public void DoesGetTransitionReturnTheExpectedStateToInTheContainer()
            {
                //Arrange
                var originalStateTo = typeof (FsmTests.StateTwo);
                _container.AddTransition(new TransitionOne(), originalStateTo);

                //Act
                var returnedTransition = _container.GetTransition<TransitionOne>().StateTo;

                //Assert
                Assert.That(returnedTransition, Is.EqualTo(originalStateTo));
            }

            [Test]
            public void DoesGetTransitionThrowTransitionNotFoundExceptionIfTheTransitionWasNotAdded()
            {
                //Arrange

                //Act

                //Assert
                Assert.Throws<TransitionNotFoundException>(() => { _container.GetTransition<TransitionOne>(); });
            }

            [Test]
            [TestCase(0, TestName = "ZeroTransitionRemove")]
            [TestCase(1, TestName = "OneTransitionRemove")]
            [TestCase(2, TestName = "TwoTransitionRemove")]
            [TestCase(3, TestName = "ThreeTransitionRemove")]
            public void DoesRemoveTransitionLowerTheTransitionCount(int amountOfTransitionsToRemove)
            {
                //Arrange
                const int maxAmount = 3;
                for (var i = 0; i < maxAmount; i++)
                    AddTransition(i);

                var expectedTransitionsLeft = maxAmount - amountOfTransitionsToRemove;

                //Act
                for (var i = 0; i < amountOfTransitionsToRemove; i++)
                    RemoveTransition(i);

                //Assert
                Assert.That(_container.TransitionCount, Is.EqualTo(expectedTransitionsLeft));
            }

            [Test]
            public void DoesRemovingATransitionThatDoesExistReturnTrue()
            {
                //Arrange
                AddSimpleTransition();
                //Act
                var removeReturnValue = _container.RemoveTransition<TransitionOne>();

                //Assert
                Assert.That(removeReturnValue, Is.True);
            }

            [Test]
            public void DoesRemovingATransitionThatDoesNotExistReturnFalse()
            {
                //Arrange

                //Act
                var removeReturnValue = _container.RemoveTransition<TransitionOne>();

                //Assert
                Assert.That(removeReturnValue, Is.False);
            }

            [Test]
            public void DoesCallingTriggerOnATransitionReturnTheStateTo()
            {
                //Arrange
                var stateThatIsSet = typeof (FsmTests.StateTwo);
                _container.AddTransition(new TransitionOne(), stateThatIsSet);

                //Act
                var stateToReturn = _container.TriggerTransition<TransitionOne>();

                //Assert
                Assert.That(stateToReturn, Is.EqualTo(stateThatIsSet));
            }

            [Test]
            public void DoesCallingTriggerOnATransitionCallTheTriggerMethod()
            {
                //Arrange
                _container.AddTransition(new TransitionOne(), typeof (FsmTests.StateTwo));

                //Act
                _container.TriggerTransition<TransitionOne>();

                //Assert
                Assert.That(_wasTriggered, Is.True);
            }

            [Test]
            public void DoesTriggerTransitionWithANonExistantTransitionThrowATransitionNotFoundException()
            {
                //Assert
                Assert.Throws<TransitionNotFoundException>(TriggerSimpleTransition);
            }

            [Test]
            public void DoesGetTransitionThrowAInvalidTransitionTypeExceptionWhenPassedAnIncorrectType()
            {
                Assert.Throws<InvalidTransitionTypeException>(GetNonTransitionClass);
            }

            [Test]
            public void DoesTiggerTransitionThrowAInvalidTransitionTypeExceptionWhenPassedAnIncorrectType()
            {
                Assert.Throws<InvalidTransitionTypeException>(TriggerNonTransitionClass);
            }
            
            private void TriggerSimpleTransition()
            {
                _container.TriggerTransition(typeof (TransitionOne));
            }

            private void GetNonTransitionClass()
            {
                _container.GetTransition(typeof (TestState));
            }

            private void TriggerNonTransitionClass()
            {
                _container.TriggerTransition(typeof (TestState));
            }

            /// <summary>
            /// Used to add a simple transition to the container using TransitionOne and StateTwo
            /// </summary>
            private void AddSimpleTransition()
            {
                _container.AddTransition(new TransitionOne(), typeof (FsmTests.StateTwo));
            }
        }

        public class TestState : IState
        {
            public int ReturnZero()
            {
                return 0;
            }

            public void OnEntry()
            {
            }

            public void OnExit()
            {
            }

            public void SetUpTransition(Action<Type> transitionMethod)
            {
            }
        }

        public class TransitionOne : ITransition
        {
            public void Trigger()
            {
                _wasTriggered = true;
            }
        }

        public class TransitionTwo : ITransition
        {
            public void Trigger()
            {
            }
        }

        public class TransitionThree : ITransition
        {
            public void Trigger()
            {
            }
        }
    }
}