CREATE PROCEDURE dbo.Project_Insert
    @name nvarchar(128),
    @projectTypeId int,
    @createdBy int,
	@numberOfChanges int = null OUTPUT
AS
BEGIN
    INSERT INTO [dbo].[Project] ([Name], [ProjectTypeId], [Operator], [Operation])
    VALUES(@name, @projectTypeId, @createdBy, N'insert')

    DECLARE @id int = SCOPE_IDENTITY()

    EXEC [dbo].[Project_GetById] @id

    SELECT @numberOfChanges = @@ROWCOUNT
END

RETURN @@ROWCOUNT