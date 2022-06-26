CREATE PROCEDURE [dbo].[ConfigurationRoot_GetById]
	@id int,
	@numberOfChanges int = null OUTPUT
AS
BEGIN
	SELECT
		cr.[Id],
		cr.[Name],
		cr.[ConfigurationTypeId],
		cr.[ProjectId],
		cr.[Operator],
		cr.[Operation],
		cr.[Timestamp]
	FROM [dbo].[ConfigurationRoot] AS cr
	WHERE cr.[Id] = @id

	SELECT @numberOfChanges = @@ROWCOUNT
END

RETURN @@ROWCOUNT
