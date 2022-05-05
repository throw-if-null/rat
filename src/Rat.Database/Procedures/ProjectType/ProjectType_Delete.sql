CREATE PROCEDURE [dbo].[ProjectType_Delete]
	@id int
AS
BEGIN
	DELETE FROM [ProjectType] WHERE Id = @id
END

RETURN @@ROWCOUNT
