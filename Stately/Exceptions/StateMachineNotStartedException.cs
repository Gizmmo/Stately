using System;

namespace Stately.Exceptions
{
    /// <summary>
    /// Thrown when a state of the type has already been added to the state machine
    /// </summary>
    public class StateMachineNotStartedException : Exception
    {
        public StateMachineNotStartedException()
        {
        }

        public StateMachineNotStartedException(string message)
            : base(message)
        {
        }

        public StateMachineNotStartedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}