CREATE PROCEDURE [dbo].[Member_SoftDelete]
	@id int,
	@modifiedBy int,
	@numberOfChanges int = null OUTPUT
AS
BEGIN
	UPDATE [dbo].[Member]
	SET
		[Deleted] = 1,
		[Operator] = @modifiedBy,
		[Operation] = N'update',
		[Timestamp] = GETUTCDATE()
	WHERE [Id] = @id

	SELECT @numberOfChanges = @@ROWCOUNT
END

RETURN @@ROWCOUNT