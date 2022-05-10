CREATE PROCEDURE [dbo].[ConfigurationRoot_GetByProjectId]
	@projectId int = 0,
	@numberOfChanges int = null OUTPUT
AS
BEGIN
	SELECT
		cr.[Id],
		cr.[Name],
		cr.[ConfigurationTypeId],
		cr.[Operator],
		cr.[Operation],
		cr.[Timestamp],
		[ConfigurationEntryCount] = [dbo].[GetConfigurationEntryCount] (cr.[Id])
	FROM [dbo].[ConfigurationRoot] AS cr
	WHERE cr.[ProjectId] = @projectId

	SELECT @numberOfChanges = @@ROWCOUNT
END

RETURN @@ROWCOUNT
