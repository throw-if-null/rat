CREATE PROCEDURE [dbo].[Member_Delete]
	@id int,
	@numberOfChanges int = null OUTPUT
AS
BEGIN
	DELETE FROM [Member] WHERE [Id] = @id

	SELECT @numberOfChanges = @@ROWCOUNT
END

RETURN @@ROWCOUNT