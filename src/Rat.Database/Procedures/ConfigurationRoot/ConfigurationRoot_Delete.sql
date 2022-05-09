CREATE PROCEDURE [dbo].[ConfigurationRoot_Delete]
	@id int,
	@numberOfChanges int = null OUTPUT
AS
BEGIN
	DELETE FROM [dbo].[ConfigurationRoot] WHERE [Id] = @id

	SELECT @numberOfChanges = @@ROWCOUNT
END

RETURN @@ROWCOUNT
