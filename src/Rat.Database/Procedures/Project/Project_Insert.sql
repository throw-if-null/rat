CREATE PROCEDURE dbo.Project_Insert
    @name nvarchar(128),
    @projectTypeId int
AS
BEGIN
    INSERT INTO [dbo].[Project] ([Name], [ProjectTypeId]) VALUES(@name, @projectTypeId)
    SELECT SCOPE_IDENTITY() AS [Id]
END

RETURN @@ROWCOUNT