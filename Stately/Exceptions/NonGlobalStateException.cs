using System;

namespace Stately.Exceptions
{

    public class NonGlobalStateException : Exception
    {
        public NonGlobalStateException()
        {
        }

        public NonGlobalStateException(string message)
            : base(message)
        {
        }

        public NonGlobalStateException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}