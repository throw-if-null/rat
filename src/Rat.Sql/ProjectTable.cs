namespace Rat.Sql
{
	internal class ProjectTable : BaseTable
	{
		public override string TableName => nameof(ProjectTable).Replace("Table", string.Empty);

		public const string Name = nameof(Name);
		public const string ProjectTypeId = nameof(ProjectTypeId);
	}
}
