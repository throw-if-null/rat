CREATE PROCEDURE [dbo].[ConfigurationEntry_Update]
	@key nvarchar(128) = NULL,
	@value nvarchar(2096) = NULL,
	@secondsToLive int = NULL,
	@disabled bit = NULL,
	@modifiedBy int,
	@id int,
	@numberOfChanges int = null OUTPUT
AS
BEGIN
	UPDATE [dbo].[ConfigurationEntry]
	SET
		[Key] = ISNULL(@key, [Key]),
		[Value] = ISNULL(@value, [Value]),
		[SecondsToLive] = ISNULL(@secondsToLive, [SecondsToLive]),
		[Disabled] = ISNULL(@disabled, [Disabled]),
		[Operator] = @modifiedBy,
		[Operation] = N'update',
		[Timestamp] = GETUTCDATE()
	WHERE
		(@key IS NOT NULL OR @value IS NOT NULL OR @secondsToLive IS NOT NULL OR @disabled IS NOT NULL) AND
		[Id] = @id

	SELECT @numberOfChanges = @@ROWCOUNT
END

RETURN @@ROWCOUNT
