CREATE PROCEDURE dbo.Member_Insert
    @authProviderId nvarchar(128),
    @createdBy int,
	@numberOfChanges int = null OUTPUT
AS
BEGIN
    INSERT INTO [dbo].[Member] ([AuthProviderId], [Operator], [Operation])
    VALUES(@authProviderId, @createdBy, N'insert')

    SELECT SCOPE_IDENTITY() AS [Id]

    SELECT @numberOfChanges = @@ROWCOUNT
END

RETURN @@ROWCOUNT
