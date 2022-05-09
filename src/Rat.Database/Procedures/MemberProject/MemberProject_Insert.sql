CREATE PROCEDURE dbo.MemberProject_Insert
    @memberId int,
    @projectId int,
	@numberOfChanges int = null OUTPUT
AS
BEGIN
    INSERT INTO [dbo].[MemberProject] ([MemberId], [ProjectId]) VALUES(@memberId, @projectId)

    SELECT @numberOfChanges = @@ROWCOUNT
END

RETURN @@ROWCOUNT