CREATE PROCEDURE [dbo].[ConfigurationRoot_GetById]
	@id int,
	@numberOfChanges int = null OUTPUT
AS
BEGIN
	SELECT
		cr.[Id],
		cr.[Name],
		cr.[ConfigurationTypeId],
		cr.[Created],
		cr.[Modified],
		cr.[CreatedBy],
		cr.[ModifiedBy]
	FROM [dbo].[ConfigurationRoot] AS cr
	WHERE cr.[Id] = @id

	SELECT @numberOfChanges = @@ROWCOUNT
END

RETURN @@ROWCOUNT
