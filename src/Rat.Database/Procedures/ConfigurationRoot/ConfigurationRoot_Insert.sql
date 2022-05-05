CREATE PROCEDURE dbo.ConfigurationRoot_Insert
    @name nvarchar(128),
    @configurationTypeId int
AS
BEGIN
    INSERT INTO [dbo].[ConfigurationRoot] ([Name], [ConfigurationTypeId]) VALUES(@name, @configurationTypeId)
    SELECT SCOPE_IDENTITY() AS [Id]
END

RETURN @@ROWCOUNT