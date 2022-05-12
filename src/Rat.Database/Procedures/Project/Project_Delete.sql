CREATE PROCEDURE [dbo].[Project_Delete]
	@id int,
	@deletedBy int,
	@numberOfChanges int = null OUTPUT
AS
BEGIN
	SELECT @numberOfChanges = @@ROWCOUNT
	BEGIN TRANSACTION

	BEGIN TRY
		ALTER TABLE [dbo].[ProjectType] SET (SYSTEM_VERSIONING = OFF);

		UPDATE [dbo].[ProjectType]
		SET
			[Operator] = @deletedBy,
			[Operation] = N'delete',
			[Timestamp] = GETUTCDATE()
		WHERE [Id] = @id

		ALTER TABLE [dbo].[ProjectType] SET
		(
			SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[ProjectHistory])
		);

		DELETE FROM [dbo].[Project] WHERE Id = @id

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
