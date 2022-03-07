using System;
using System.Runtime.Serialization;

namespace Rat.Data.Exceptions
{
	[Serializable]
	public sealed class OopsieException : Exception
	{
		public OopsieException(Exception innerException)
			: base("Unhandled exception occurred", innerException)
		{
		}

		private OopsieException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}
	}
}
