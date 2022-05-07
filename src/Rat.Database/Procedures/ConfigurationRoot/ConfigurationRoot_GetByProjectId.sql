CREATE PROCEDURE [dbo].[ConfigurationRoot_GetByProjectId]
	@projectId int = 0
AS
BEGIN
	SELECT
		cr.[Id],
		cr.[Name],
		cr.[ConfigurationTypeId],
		cr.[Created],
		cr.[Modified],
		cr.[CreatedBy],
		cr.[ModifiedBy],
		[ConfigurationEntryCount] = [dbo].[GetConfigurationEntryCount] (cr.[Id])
	FROM [dbo].[ConfigurationRoot] AS cr
	WHERE cr.[ProjectId] = @projectId
END

RETURN @@ROWCOUNT
