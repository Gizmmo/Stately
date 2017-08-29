namespace Stately.Tests.MockData
{
    public enum TransitionActions
    {
        TriggerOne,
        TriggerTwo,
        TriggerThree
    }

    public class TransitionOne : Transition
    {
        public bool IsTriggered;

        /// <inheritdoc />
        public override void Trigger() => IsTriggered = true;
    }

    public class TransitionTwo : Transition
    {
        /// <inheritdoc />
        public override void Trigger()
        {
        }
    }

    public class TransitionThree : Transition
    {
        /// <inheritdoc />
        public override void Trigger()
        {
        }
    }
}