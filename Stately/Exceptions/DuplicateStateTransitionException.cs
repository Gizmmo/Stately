using System;

namespace Stately.Exceptions
{
    /// <summary>
    /// Thrown when a state of the type has already been added to the state machine
    /// </summary>
    public class DuplicateStateTransitionException : Exception
    {
        public DuplicateStateTransitionException()
        {
        }

        public DuplicateStateTransitionException(string message)
            : base(message)
        {
        }

        public DuplicateStateTransitionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}