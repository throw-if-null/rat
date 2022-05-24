CREATE PROCEDURE [dbo].[Member_GetByAuthProviderId]
	@authProviderId nvarchar(128),
	@numberOfChanges int = NULL OUTPUT

AS
BEGIN
	SELECT
		m.[Id],
		m.[AuthProviderId],
		m.[Deleted],
		m.[Timestamp],
		m.[Operator],
		m.[Operation]
	FROM [dbo].[Member] AS m
	WHERE
		m.[AuthProviderId] = @authProviderId

	SELECT @numberOfChanges = @@ROWCOUNT
END

RETURN @@ROWCOUNT
