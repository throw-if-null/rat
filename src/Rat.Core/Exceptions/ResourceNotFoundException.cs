using System;
using System.Runtime.Serialization;

namespace Rat.Core.Exceptions
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
