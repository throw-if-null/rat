CREATE PROCEDURE [dbo].[Project_GetById]
	@id int,
	@numberOfChanges int = null OUTPUT
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[ProjectTypeId],
		[Created],
		[Modified],
		[CreatedBy],
		[ModifiedBy],
		[ConfigurationCount] = [dbo].[GetConfigurationRootCount] ([Id]),
		[EntriesCount] = [dbo].[GetProjectConfigurationEntryCount] ([Id])
	FROM [dbo].[Project]
	WHERE [Id] = @id

	SELECT @numberOfChanges = @@ROWCOUNT
END

RETURN @@ROWCOUNT
