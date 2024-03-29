﻿CREATE PROCEDURE [dbo].[ProjectType_Update]
	@name nvarchar(64),
	@id int,
	@modifiedBy int,
	@numberOfChanges int = null OUTPUT
AS
BEGIN
	UPDATE [ProjectType]
	SET
		[Name] = @name,
		[Operator] = @modifiedBy,
		[Operation] = N'update',
		[Timestamp] = GETUTCDATE()
	WHERE
		[Id] = @id

	SELECT @numberOfChanges = @@ROWCOUNT
END

RETURN @@ROWCOUNT
