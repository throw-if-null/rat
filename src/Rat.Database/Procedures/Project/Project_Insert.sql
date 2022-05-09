CREATE PROCEDURE dbo.Project_Insert
    @name nvarchar(128),
    @projectTypeId int,
	@numberOfChanges int = null OUTPUT
AS
BEGIN
    INSERT INTO [dbo].[Project] ([Name], [ProjectTypeId]) VALUES(@name, @projectTypeId)
    SELECT SCOPE_IDENTITY() AS [Id]

    SELECT @numberOfChanges = @@ROWCOUNT
END

RETURN @@ROWCOUNT