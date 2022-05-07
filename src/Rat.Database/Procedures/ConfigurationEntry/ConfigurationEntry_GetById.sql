﻿CREATE PROCEDURE [dbo].[ConfigurationEntry_GetById]
	@id int
AS
BEGIN
	SELECT
		ce.[Id],
		ce.[Key],
		ce.[Value],
		ce.[SecondsToLive],
		ce.[Disabled],
		ce.[Created],
		ce.[Modified],
		ce.[CreatedBy],
		ce.[ModifiedBy]
	FROM [dbo].[ConfigurationEntry] AS ce
	WHERE ce.[Id] = @id
END

RETURN @@ROWCOUNT
