CREATE PROCEDURE dbo.ProjectType_GetAll
AS
BEGIN
    SELECT [Id], [Name] FROM [dbo].[ProjectType]
END

RETURN @@ROWCOUNT