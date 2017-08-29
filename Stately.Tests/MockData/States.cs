using System;

namespace Stately.Tests.MockData
{
    public abstract class ConcreteState : IState
        {
            public bool IsEntryCalled { get; private set; }
            public bool IsExitCalled { get; private set; }
            public int AmountOfEntryCalls { get; private set; }
            public int AmountOfExitCalls { get; private set; }

            private readonly EntryExitTestObject _testObject;

            protected ConcreteState()
            {
            }

            protected ConcreteState(EntryExitTestObject testObject)
            {
                _testObject = testObject;
            }

            public int ReturnZero()
            {
                return 0;
            }

            public abstract int ReturnStateNumber();

            public virtual void OnEntry()
            {
                AmountOfEntryCalls++;
                IsEntryCalled = true;
                _testObject?.OnEntry();
            }

            public virtual void OnExit()
            {
                AmountOfExitCalls++;
                IsExitCalled = true;
                _testObject?.OnExit();
            }

            public void SetUpTransition(Action<Type> transitionMethod)
            {
            }
        }

        public class StateOne : ConcreteState
        {
            public override int ReturnStateNumber()
            {
                return 1;
            }

            public StateOne()
            {
            }

            public StateOne(EntryExitTestObject testObject) : base(testObject)
            {
            }
        }

        public class StateTwo : ConcreteState
        {
            public bool IsUniqueEntryCalled { get; private set; }

            public override int ReturnStateNumber()
            {
                return 2;
            }

            public override void OnEntry()
            {
                base.OnEntry();
                IsUniqueEntryCalled = true;
            }

            public StateTwo(EntryExitTestObject testObject) : base(testObject)
            {
            }

            public StateTwo()
            {
            }
        }

        public class StateThree : ConcreteState
        {
            public override int ReturnStateNumber()
            {
                return 3;
            }

            public StateThree(EntryExitTestObject testObject) : base(testObject)
            {
            }

            public StateThree()
            {
            }
        }

        public class StateFour : ConcreteState
        {
            public override int ReturnStateNumber()
            {
                return 4;
            }

            public StateFour(EntryExitTestObject testObject) : base(testObject)
            {
            }

            public StateFour()
            {
            }
        }

        public class StateFive : ConcreteState
        {
            public override int ReturnStateNumber()
            {
                return 5;
            }

            public StateFive(EntryExitTestObject testObject) : base(testObject)
            {
            }

            public StateFive()
            {
            }
        }
}