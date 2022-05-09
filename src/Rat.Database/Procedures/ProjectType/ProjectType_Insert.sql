CREATE PROCEDURE dbo.ProjectType_Insert
    @name nvarchar(64),
    @createdBy int,
    @numberOfChanges int = null OUTPUT
AS
BEGIN
    INSERT INTO [dbo].[ProjectType] ([Name], [CreatedBy], [ModifiedBy])
    VALUES(@name, @createdBy, @createdBy)

    SELECT SCOPE_IDENTITY() AS [Id]

    SELECT @numberOfChanges = @@ROWCOUNT
END

RETURN @@ROWCOUNT
