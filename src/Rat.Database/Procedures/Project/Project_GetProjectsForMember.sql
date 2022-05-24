CREATE PROCEDURE [dbo].[Project_GetProjectsForMember]
	@memberId int,
	@numberOfChanges int = null OUTPUT
AS
BEGIN
	SELECT
		p.[Id],
		p.[Name],
		p.[ProjectTypeId],
		p.[Operator],
		p.[Operation],
		p.[Timestamp]
	FROM [dbo].[MemberProject] AS mp
	INNER JOIN [dbo].[Project] AS p
		ON mp.[ProjectId] = p.[Id]
	WHERE mp.[MemberId] = @memberId

	SELECT @numberOfChanges = @@ROWCOUNT
END

RETURN @@ROWCOUNT
