using System;

namespace Stately.Exceptions
{
    /// <summary>
    /// Thrown when a state of the type has already been added to the state machine
    /// </summary>
    public class TransitionNotFoundException : Exception
    {
        public TransitionNotFoundException()
        {
        }

        public TransitionNotFoundException(string message)
            : base(message)
        {
        }

        public TransitionNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}