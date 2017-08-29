using NUnit.Framework;
using Stately.Exceptions;
using Stately.Tests.MockData;

namespace Stately.Tests
{
    [TestFixture]
    [Category("FsmStateContainer")]
    public class StateContainerTests
    {
        private StateContainer<ConcreteState> _container;

        [SetUp]
        public virtual void Init()
        {
        }

        protected void AddTransition(int transitionNum)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (transitionNum)
            {
                case 0:
                    _container.AddTransition(new TransitionOne(), typeof (StateTwo));
                    break;
                case 1:
                    _container.AddTransition(new TransitionTwo(), typeof (StateTwo));
                    break;
                case 2:
                    _container.AddTransition(new TransitionThree(), typeof (StateTwo));
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
                var stateType = typeof (StateOne);
                //Act
                _container = new StateContainer<ConcreteState>(new StateOne());
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
                _container = new StateContainer<ConcreteState>(new StateOne());
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
                _container.AddTransition(originalTransition, typeof (StateTwo));

                //Act
                var returnedTransition = _container.GetTransition<TransitionOne>().Transition;

                //Assert
                Assert.That(returnedTransition, Is.EqualTo(originalTransition));
            }

            [Test]
            public void DoesGetTransitionReturnTheExpectedStateToInTheContainer()
            {
                //Arrange
                var originalStateTo = typeof (StateTwo);
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
                var stateThatIsSet = typeof (StateTwo);
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
                var transition = new TransitionOne();
                _container.AddTransition(transition, typeof (StateTwo));

                //Act
                _container.TriggerTransition<TransitionOne>();

                //Assert
                Assert.That(transition.IsTriggered, Is.True);
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
            
            [Test]
            public void HasTransitionReturnsTrueIfATransitionWasAddedToAnotherState()
            {
                //Arrange
                var transition = new TransitionOne();
                _container.AddTransition(transition, typeof (StateTwo));
            
                //Act
                var hasTransitionReturnsTrue = _container.HasTransition<StateTwo>();
                    
                //Assert
                Assert.That(hasTransitionReturnsTrue, Is.True);
            
            }
            
            [Test]
            public void HasTransitionReturnsFalseIfATransitionWasNotAddedToAnotherState()
            {
                //Arrange
            
                //Act
                var hasTransitionReturnsFalse = _container.HasTransition<StateTwo>();
                    
                //Assert
                Assert.That(hasTransitionReturnsFalse, Is.False);
            
            }
            
            private void TriggerSimpleTransition()
            {
                _container.TriggerTransition(typeof (TransitionOne));
            }

            private void GetNonTransitionClass()
            {
                _container.GetTransition(typeof (StateOne));
            }

            private void TriggerNonTransitionClass()
            {
                _container.TriggerTransition(typeof (StateOne));
            }

            /// <summary>
            /// Used to add a simple transition to the container using TransitionOne and StateTwo
            /// </summary>
            private void AddSimpleTransition()
            {
                _container.AddTransition(new TransitionOne(), typeof (StateTwo));
            }
        }
    }
}