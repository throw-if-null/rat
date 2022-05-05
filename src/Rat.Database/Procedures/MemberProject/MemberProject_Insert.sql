CREATE PROCEDURE dbo.MemberProject_Insert
    @memberId int,
    @projectId int
AS
BEGIN
    INSERT INTO [dbo].[MemberProject] ([MemberId], [ProjectId]) VALUES(@memberId, @projectId)
END

RETURN @@ROWCOUNT