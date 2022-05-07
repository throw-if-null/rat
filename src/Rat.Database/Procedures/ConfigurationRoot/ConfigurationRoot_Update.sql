CREATE PROCEDURE [dbo].[ConfigurationRoot_Update]
	@name nvarchar(64) = NULL,
	@configurationTypeId int = NULL,
	@modifiedBy int,
	@id int
AS
BEGIN
	UPDATE [dbo].[ConfigurationRoot]
	SET
		[Name] = ISNULL(@name, [Name]),
		[ConfigurationTypeId] = ISNULL(@configurationTypeId, [ConfigurationTypeId]),
		[ModifiedBy] = @modifiedBy,
		[Modified] = GETUTCDATE()
	WHERE
		(@name IS NOT NULL OR @configurationTypeId IS NOT NULL) AND
		[Id] = @id
END

RETURN @@ROWCOUNT
