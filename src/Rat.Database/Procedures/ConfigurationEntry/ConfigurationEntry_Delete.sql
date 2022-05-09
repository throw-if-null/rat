CREATE PROCEDURE [dbo].[ConfigurationEntry_Delete]
	@id int,
	@numberOfChanges int = null OUTPUT
AS
BEGIN
	DELETE FROM [dbo].[ConfigurationEntry] WHERE Id = @id

	SELECT @numberOfChanges = @@ROWCOUNT
END

RETURN @@ROWCOUNT
