CREATE PROCEDURE dbo.ProjectType_GetAll(
	@numberOfChanges int = null OUTPUT
)
AS
BEGIN
    SELECT [Id], [Name] FROM [dbo].[ProjectType]

	SELECT @numberOfChanges = @@ROWCOUNT
END

RETURN @@ROWCOUNT