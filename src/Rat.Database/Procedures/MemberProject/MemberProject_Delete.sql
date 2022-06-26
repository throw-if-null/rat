CREATE PROCEDURE [dbo].[MemberProject_Delete]
	@memberId int,
	@projectId int,
	@deletedBy int,
	@numberOfChanges int NULL OUTPUT
AS
BEGIN
	SELECT @numberOfChanges = @@ROWCOUNT
		BEGIN TRANSACTION

		BEGIN TRY
			UPDATE [dbo].[MemberProject]
			SET
				[Operator] = @deletedBy,
				[Operation] = N'delete',
				[Timestamp] = GETUTCDATE()
			WHERE
				[MemberId] = @memberId AND
				[ProjectId] = @projectId

			DELETE FROM [dbo].[MemberProject] WHERE [MemberId] = @memberId AND [ProjectId] = @projectId

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
