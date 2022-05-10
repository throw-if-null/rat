CREATE PROCEDURE [dbo].[ProjectType_Delete]
	@id int,
	@numberOfChanges int = NULL OUTPUT
AS
BEGIN
	DELETE FROM [ProjectType] WHERE Id = @id

	SELECT @numberOfChanges = @@ROWCOUNT
END

RETURN @@ROWCOUNT
