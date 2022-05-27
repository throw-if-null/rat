using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Rat.Core.Exceptions
{
	[Serializable]
	public sealed class InvalidRequestDataException : Exception
	{
		public InvalidRequestDataException(KeyValuePair<string, string> validationError)
			: this(new[] { validationError })
		{
		}

		public InvalidRequestDataException(KeyValuePair<string, string>[] validationErrors)
			: this(validationErrors.ToDictionary(x => x.Key, x => x.Value))
		{
		}

		public InvalidRequestDataException(IDictionary<string, string> validationErrors)
			: base("Bad request")
		{
			Data["ValidationErrors"] = validationErrors;
		}

		private InvalidRequestDataException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}
	}
}
