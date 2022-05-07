CREATE PROCEDURE [dbo].[ConfigurationRoot_Delete]
	@id int
AS
BEGIN
	DELETE FROM [dbo].[ConfigurationRoot] WHERE [Id] = @id
END

RETURN @@ROWCOUNT
