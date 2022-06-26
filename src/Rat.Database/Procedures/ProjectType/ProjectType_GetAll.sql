CREATE PROCEDURE dbo.ProjectType_GetAll(
	@numberOfChanges int = null OUTPUT
)
AS
BEGIN
    SELECT
		pt.[Id],
		pt.[Name],
		pt.[Operator],
		pt.[Operation],
		pt.[Timestamp]
	FROM [dbo].[ProjectType] AS pt

	SELECT @numberOfChanges = @@ROWCOUNT
END

RETURN @@ROWCOUNT