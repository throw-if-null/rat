CREATE PROCEDURE [dbo].[ProjectType_UpdateName]
	@name nvarchar(64),
	@id int
AS
BEGIN
	UPDATE [ProjectType]
	SET [Name] = @name
	WHERE Id = @id
END

RETURN @@ROWCOUNT
