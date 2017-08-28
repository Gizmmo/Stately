using System;

namespace Stately.Exceptions
{
    /// <summary>
    /// Thrown when an inital state is not set for start up.
    /// </summary>
    public class InitalStateNullException : Exception
    {
        public InitalStateNullException()
        {
        }

        public InitalStateNullException(string message)
            : base(message)
        {
        }

        public InitalStateNullException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}