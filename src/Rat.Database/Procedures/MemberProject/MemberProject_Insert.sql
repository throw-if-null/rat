CREATE PROCEDURE dbo.MemberProject_Insert
    @memberId int,
    @projectId int,
    @createdBy int,
	@numberOfChanges int = null OUTPUT
AS
BEGIN
    INSERT INTO [dbo].[MemberProject] ([MemberId], [ProjectId], [Operator], [Operation])
    VALUES(@memberId, @projectId, @createdBy, N'insert')

    SELECT @numberOfChanges = @@ROWCOUNT
END

RETURN @@ROWCOUNT