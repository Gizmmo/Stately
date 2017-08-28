using System;

namespace Stately.Exceptions
{
    /// <summary>
    /// Thrown when a state is being removed that is set as the current state.
    /// </summary>
    public class RemoveCurrentStateException : Exception
    {
        public RemoveCurrentStateException()
        {
        }

        public RemoveCurrentStateException(string message)
            : base(message)
        {
        }

        public RemoveCurrentStateException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}