CREATE PROCEDURE [dbo].[ConfigurationEntry_GetByConfigurationRootId]
	@configurationRootId int
AS
BEGIN
	SELECT
		ce.[Id],
		ce.[Key],
		ce.[Value],
		ce.[Disabled],
		ce.[SecondsToLive],
		ce.[Created],
		ce.[Modified],
		ce.[CreatedBy],
		ce.[ModifiedBy]
	FROM [dbo].[ConfigurationEntry] AS ce
	WHERE [ConfigurationRootId] = @configurationRootId
	ORDER BY ce.[Created] ASC
END

RETURN @@ROWCOUNT
