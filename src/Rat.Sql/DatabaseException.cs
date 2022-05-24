using System.Runtime.Serialization;

namespace Rat.Sql
{
	[Serializable]
	internal class DatabaseException : Exception
	{
		private DatabaseException()
		{
		}

		internal DatabaseException(string? message) : base(message)
		{
		}

		internal DatabaseException(string? message, Exception? innerException) : base(message, innerException)
		{
		}

		protected DatabaseException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		public DatabaseException(int noc, int expectedNoc)
			: base($"Actual number of changes: {noc} and expected: {expectedNoc} don't match")
		{
		}
	}
}