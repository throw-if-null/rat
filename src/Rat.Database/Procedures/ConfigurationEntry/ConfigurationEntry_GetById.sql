CREATE PROCEDURE [dbo].[ConfigurationEntry_GetById]
	@id int
AS
BEGIN
	SELECT [Id], [Key], [Value], [SecondsToLive], [Disabled], [Created], [Modified], [CreatedBy], [ModifiedBy]
	FROM [dbo].[ConfigurationEntry]
	WHERE Id = @id
END

RETURN @@ROWCOUNT
