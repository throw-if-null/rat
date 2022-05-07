CREATE PROCEDURE [dbo].[Project_GetProjectsForMember]
	@memberId int
AS
BEGIN
	SELECT
		p.[Id],
		p.[Name],
		p.[ProjectTypeId],
		p.[Created],
		p.[Modified],
		p.[CreatedBy],
		p.[ModifiedBy]
	FROM [dbo].[MemberProject] AS mp
	INNER JOIN [dbo].[Project] AS p
	ON mp.[ProjectId] = p.[Id]
	WHERE mp.[MemberId] = @memberId
END

RETURN @@ROWCOUNT
