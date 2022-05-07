CREATE PROCEDURE [dbo].[Member_Delete]
	@id int
AS
BEGIN
	DELETE FROM [Member] WHERE [Id] = @id
END

RETURN @@ROWCOUNT