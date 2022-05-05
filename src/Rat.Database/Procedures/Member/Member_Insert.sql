CREATE PROCEDURE dbo.Member_Insert
    @authProviderId nvarchar(128)
AS
BEGIN
    INSERT INTO [dbo].[Member] ([AuthProviderId]) VALUES(@authProviderId)
    SELECT SCOPE_IDENTITY() AS [Id]
END

RETURN @@ROWCOUNT