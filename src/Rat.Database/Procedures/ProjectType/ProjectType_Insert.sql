CREATE PROCEDURE dbo.ProjectType_Insert
    @name nvarchar(64)
AS
BEGIN
    INSERT INTO [dbo].[ProjectType] ([Name]) VALUES(@name)
    SELECT SCOPE_IDENTITY() AS [Id]
END

RETURN @@ROWCOUNT