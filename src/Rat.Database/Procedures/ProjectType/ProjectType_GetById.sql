CREATE PROCEDURE [dbo].[ProjectType_GetById]
	@id int
AS
BEGIN
	SELECT [Id], [Name] FROM [dbo].[ProjectType] WHERE Id = @id
END

RETURN @@ROWCOUNT
