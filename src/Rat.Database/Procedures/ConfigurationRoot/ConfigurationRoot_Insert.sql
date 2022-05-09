CREATE PROCEDURE dbo.ConfigurationRoot_Insert
    @name nvarchar(128),
    @configurationTypeId int,
	@numberOfChanges int = null OUTPUT
AS
BEGIN
    INSERT INTO [dbo].[ConfigurationRoot] ([Name], [ConfigurationTypeId]) VALUES(@name, @configurationTypeId)
    SELECT SCOPE_IDENTITY() AS [Id]

    SELECT @numberOfChanges = @@ROWCOUNT
END

RETURN @@ROWCOUNT