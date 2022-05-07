CREATE FUNCTION [dbo].[GetProjectConfigurationEntryCount]
(
	@projectId int
)
RETURNS INT
AS
BEGIN
	DECLARE @count int

	SELECT @count = COUNT(*)
	FROM [dbo].[ConfigurationEntry] AS ce
	INNER JOIN [dbo].[ConfigurationRoot] AS cr
	ON ce.[ConfigurationRootId] = cr.[Id]
	WHERE cr.[ProjectId] = @projectId

	RETURN @count
END
