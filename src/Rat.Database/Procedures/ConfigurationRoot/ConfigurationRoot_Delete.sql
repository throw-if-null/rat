CREATE PROCEDURE [dbo].[ConfigurationRoot_Delete]
	@id int,
	@deletedBy int,
	@numberOfChanges int = null OUTPUT
AS
BEGIN
	BEGIN TRANSACTION

	BEGIN TRY
		ALTER TABLE [dbo].[ProjectType] SET (SYSTEM_VERSIONING = OFF);

		UPDATE [dbo].[ConfigurationRoot]
		SET
			[Operator] = @deletedBy,
			[Operation] = N'delete',
			[Timestamp] = GETUTCDATE()
		WHERE [Id] = @id

		ALTER TABLE [dbo].[ConfigurationRoot] SET
		(
			SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[ConfigurationRootHistory])
		);

		DELETE FROM [dbo].[ConfigurationRoot] WHERE [Id] = @id

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
