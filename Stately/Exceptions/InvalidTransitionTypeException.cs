using System;

namespace Stately.Exceptions
{
    public class InvalidTransitionTypeException : Exception
    {
        public InvalidTransitionTypeException()
        {
        }

        public InvalidTransitionTypeException(string message)
            : base(message)
        {
        }

        public InvalidTransitionTypeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}