CREATE PROCEDURE [dbo].[Project_GetById]
	@id int
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
END

RETURN @@ROWCOUNT
