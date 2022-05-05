namespace Rat.Sql
{
	internal abstract class BaseTable
	{
		public abstract string TableName { get; }

		public const string Id = nameof(Id);
		public const string Created = nameof(Created);
		public const string Modified = nameof(Modified);
		public const string Deleted = nameof(Deleted);
	}
}
