using System;

namespace Stately
{
    public class TransitionContainer : ITransitionContainer
    {
        public TransitionContainer(ITransition transition, Type stateTo)
        {
            StateTo = stateTo;
            Transition = transition;
        }

        public Type StateTo { get; }

        public ITransition Transition { get; }
    }
}