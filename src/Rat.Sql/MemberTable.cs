namespace Rat.Sql
{
	internal class MemberTable : BaseTable
	{
		public override string TableName => nameof(MemberTable).Replace("Table", string.Empty);

		public const string AuthProviderId = nameof(AuthProviderId);
	}
}
