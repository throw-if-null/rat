CREATE PROCEDURE dbo.ConfigurationEntry_Insert
    @configurationRootId int,
    @key nvarchar(128),
    @value nvarchar(2048),
    @secondsToLive int,
    @disabled bit,
    @createdBy int,
	@numberOfChanges int = null OUTPUT
AS
BEGIN
    INSERT INTO [dbo].[ConfigurationEntry] ([ConfigurationRootId], [Key], [Value], [SecondsToLive], [Disabled], [CreatedBy], [ModifiedBy])
    VALUES(@configurationRootId, @key, @value, @secondsToLive, @disabled, @createdBy, @createdBy)

    SELECT SCOPE_IDENTITY() AS [Id]

    SELECT @numberOfChanges = @@ROWCOUNT
END

RETURN @@ROWCOUNT
