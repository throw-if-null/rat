CREATE PROCEDURE dbo.Member_Insert
    @authProviderId nvarchar(128),
	@numberOfChanges int = null OUTPUT
AS
BEGIN
    INSERT INTO [dbo].[Member] ([AuthProviderId]) VALUES(@authProviderId)
    SELECT SCOPE_IDENTITY() AS [Id]

    SELECT @numberOfChanges = @@ROWCOUNT
END

RETURN @@ROWCOUNT