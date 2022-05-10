CREATE PROCEDURE [dbo].[ConfigurationEntry_GetByConfigurationRootId]
	@configurationRootId int,
	@numberOfChanges int = null OUTPUT
AS
BEGIN
	SELECT
		ce.[Id],
		ce.[Key],
		ce.[Value],
		ce.[Disabled],
		ce.[SecondsToLive],
		ce.[Operator],
		ce.[Operation],
		ce.[Timestamp]
	FROM [dbo].[ConfigurationEntry] AS ce
	WHERE [ConfigurationRootId] = @configurationRootId
	ORDER BY ce.[Timestamp] ASC

	SELECT @numberOfChanges = @@ROWCOUNT
END

RETURN @@ROWCOUNT
