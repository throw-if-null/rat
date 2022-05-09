CREATE PROCEDURE [dbo].[Project_Update]
	@name nvarchar(128) = NULL,
	@projectTypeId int = NULL,
	@id int,
	@modifiedBy int,
	@numberOfChanges int = null OUTPUT
AS
BEGIN
	UPDATE [dbo].[Project]
	SET
		[Name] = ISNULL(@name, [Name]),
		[ProjectTypeId] = ISNULL(@projectTypeId, [ProjectTypeId]),
		[ModifiedBy] = @modifiedBy,
		[Modified] = GETUTCDATE()
	WHERE
		[Id] = @id

	SELECT @numberOfChanges = @@ROWCOUNT
END

RETURN @@ROWCOUNT
