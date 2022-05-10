CREATE PROCEDURE dbo.ProjectType_Insert
    @name nvarchar(64),
    @createdBy int,
    @numberOfChanges int = null OUTPUT
AS
BEGIN
    INSERT INTO [dbo].[ProjectType] ([Name], [Operator], [Operation])
    VALUES(@name, @createdBy, N'insert')

    SELECT SCOPE_IDENTITY() AS [Id]

    SELECT @numberOfChanges = @@ROWCOUNT
END

RETURN @@ROWCOUNT
