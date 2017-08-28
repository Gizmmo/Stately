using System;

namespace Stately.Exceptions
{
    /// <summary>
    /// Thrown when a state does not exist is trying to be accessed.
    /// </summary>
    public class StateNotFoundException : Exception
    {
        public StateNotFoundException()
        {
        }

        public StateNotFoundException(string message)
            : base(message)
        {
        }

        public StateNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}