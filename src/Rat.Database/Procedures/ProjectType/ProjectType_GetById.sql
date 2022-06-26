CREATE PROCEDURE [dbo].[ProjectType_GetById]
	@id int,
	@numberOfChanges int = null OUTPUT
AS
BEGIN
	SELECT
		pt.[Id],
		pt.[Name],
		pt.[Operator],
		pt.[Operation],
		pt.[Timestamp]
	FROM [dbo].[ProjectType] AS pt
	WHERE Id = @id

	SELECT @numberOfChanges = @@ROWCOUNT
END

RETURN @@ROWCOUNT
