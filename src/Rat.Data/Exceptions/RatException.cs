using System;
using System.Runtime.Serialization;

namespace Rat.Data.Exceptions
{
    [Serializable]
    public sealed class RatException : Exception
    {
        public RatException(Exception innerException)
             : base("Unhandled exception occurred", innerException)
        {
        }

        public RatException() : this(innerException: null)
        {
        }

        public RatException(string message)
            : this(innerException: null)
        {
        }

        public RatException(string message, Exception innerException)
            : this(innerException: innerException)
        {
        }

        private RatException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
