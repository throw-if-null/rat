CREATE FUNCTION [dbo].[GetConfigurationRootCount]
(
	@projectId int
)
RETURNS INT
AS
BEGIN
	DECLARE @count int

	SELECT @count = COUNT(*) FROM [dbo].[ConfigurationRoot] WHERE [ProjectId] = @projectId

	RETURN @count
END
