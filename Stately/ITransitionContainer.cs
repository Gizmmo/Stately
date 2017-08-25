using System;

namespace Stately
{
    public interface ITransitionContainer
    {
        /// <summary>
        /// The state that the transition will go to afterwards
        /// </summary>
        Type StateTo { get; }

        /// <summary>
        /// The transition to be used
        /// </summary>
        ITransition Transition { get; }

    }
}