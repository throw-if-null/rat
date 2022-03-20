using System;
using System.Runtime.Serialization;

namespace Rat.Core.Exceptions
{
    [Serializable]
    public sealed class RatDbException : Exception
    {
        public RatDbException(string message) : base(message)
        {
        }

        private RatDbException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
