namespace Stately.Tests.MockData
{   
    public class TransitionOne : FsmTransition
    {
        public bool IsTriggered;

        /// <inheritdoc />
        public override void Trigger() => IsTriggered = true;
    }

    public class TransitionTwo : FsmTransition
    {
        /// <inheritdoc />
        public override void Trigger()
        {
        }
    }

    public class TransitionThree : FsmTransition
    {
        /// <inheritdoc />
        public override void Trigger()
        {
        }
    }
}