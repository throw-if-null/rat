CREATE FUNCTION [dbo].[GetConfigurationEntryCount]
(
	@configurationRootId int
)
RETURNS INT
AS
BEGIN
	DECLARE @entries INT

	SELECT @entries = COUNT(*)
	FROM [dbo].[ConfigurationEntry]
	WHERE [ConfigurationRootId] = @configurationRootId

	RETURN @entries
END
