using NUnit.Framework;
using Stately.Tests.MockData;

namespace Stately.Tests
{
    [TestFixture]
    [Category("FsmTransitionContainer")]
    public class TransitionContainerTests
    {
        private TransitionContainer _container;

        [SetUp]
        public void Init()
        {
            _container = new TransitionContainer(new TransitionOne(), typeof (StateTwo));
        }

        [Test]
        public void DoesInitalizingTheTransitionContiainerSetTheTransitonPropertyToAnIntializedValue()
        {
            //Arrange

            //Act

            //Assert
            Assert.That(_container.Transition, Is.Not.Null);
        }

        [Test]
        public void DoesInitalizingTheTransitionContiainerSetTheTransitonPropertyToTheCorrectType()
        {
            //Arrange
            var containerTransitionType = _container.Transition.GetType();
            var transitionPassedType = typeof (TransitionOne);

            //Act

            //Assert
            Assert.That(containerTransitionType, Is.EqualTo(transitionPassedType));
        }

        [Test]
        public void DoesInitalizingTheTransitionContiainerSetTheStateToPropertyToTheCorrectType()
        {
            //Arrange
            var stateToPassedType = typeof (StateTwo);

            //Act

            //Assert
            Assert.That(_container.StateTo, Is.EqualTo(stateToPassedType));
        }
    }
}