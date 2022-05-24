CREATE PROCEDURE dbo.ConfigurationRoot_Insert
    @projectId int,
    @name nvarchar(128),
    @configurationTypeId int,
    @createdBy int,
	@numberOfChanges int = null OUTPUT
AS
BEGIN
    INSERT INTO [dbo].[ConfigurationRoot] ([Name], [ConfigurationTypeId], [ProjectId], [Operator], [Operation])
    VALUES(@name, @configurationTypeId, @projectId, @createdBy, N'insert')

    SELECT SCOPE_IDENTITY() AS [Id]

    SELECT @numberOfChanges = @@ROWCOUNT
END

RETURN @@ROWCOUNT