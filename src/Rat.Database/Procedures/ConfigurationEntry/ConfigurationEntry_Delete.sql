CREATE PROCEDURE [dbo].[ConfigurationEntry_Delete]
	@id int
AS
BEGIN
	DELETE FROM [dbo].[ConfigurationEntry] WHERE Id = @id
END

RETURN @@ROWCOUNT
