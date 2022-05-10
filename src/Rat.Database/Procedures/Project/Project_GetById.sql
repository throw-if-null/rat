CREATE PROCEDURE [dbo].[Project_GetById]
	@id int,
	@numberOfChanges int = null OUTPUT
AS
BEGIN
	SELECT
		p.[Id],
		p.[Name],
		p.[ProjectTypeId],
		p.[Operator],
		p.[Operation],
		p.[Timestamp],
		[ConfigurationCount] = [dbo].[GetConfigurationRootCount] ([Id]),
		[EntriesCount] = [dbo].[GetProjectConfigurationEntryCount] ([Id])
	FROM [dbo].[Project] AS p
	WHERE [Id] = @id

	SELECT @numberOfChanges = @@ROWCOUNT
END

RETURN @@ROWCOUNT
