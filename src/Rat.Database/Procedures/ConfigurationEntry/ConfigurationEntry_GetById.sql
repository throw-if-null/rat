CREATE PROCEDURE [dbo].[ConfigurationEntry_GetById]
	@id int,
	@numberOfChanges int = null OUTPUT
AS
BEGIN
	SELECT
		ce.[Id],
		ce.[Key],
		ce.[Value],
		ce.[SecondsToLive],
		ce.[Disabled],
		ce.[Operator],
		ce.[Operation],
		ce.[Timestamp]
	FROM [dbo].[ConfigurationEntry] AS ce
	WHERE ce.[Id] = @id

	SELECT @numberOfChanges = @@ROWCOUNT
END

RETURN @@ROWCOUNT
