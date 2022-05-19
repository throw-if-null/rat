CREATE PROCEDURE [dbo].[ProjectType_Delete]
	@id int,
	@deletedBy int,
	@numberOfChanges int = NULL OUTPUT
AS
BEGIN
	BEGIN TRANSACTION

	BEGIN TRY
		UPDATE [dbo].[ProjectType]
		SET
			[Operator] = @deletedBy,
			[Operation] = N'delete',
			[Timestamp] = GETUTCDATE()
		WHERE [Id] = @id

		DELETE FROM [ProjectType] WHERE Id = @id

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION
		END

		DECLARE @error nvarchar(2048) = error_message()
		RAISERROR(@error, 16, 1)
	END CATCH

	SELECT @numberOfChanges = @@ROWCOUNT
END

RETURN @@ROWCOUNT
