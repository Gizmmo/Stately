using System;

namespace Stately.Exceptions
{
    /// <summary>
    /// Thrown when a state of the type has already been added to the state machine
    /// </summary>
    public class DuplicateStateException : Exception
    {
        public DuplicateStateException()
        {
        }

        public DuplicateStateException(string message)
            : base(message)
        {
        }

        public DuplicateStateException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}