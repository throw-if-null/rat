using System;
using System.Runtime.Serialization;

namespace Rat.Data.Exceptions
{
	[Serializable]
	public sealed class ResourceNotFoundException : Exception
	{
		public ResourceNotFoundException(string message) : base(message)
		{
		}

		private ResourceNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}
	}
}
