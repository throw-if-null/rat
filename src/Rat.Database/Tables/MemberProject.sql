CREATE TABLE [dbo].[MemberProject]
(
	[MemberId] INT NOT NULL,
	[ProjectId] INT NOT NULL,

	[Created] DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(), 
    [Modified] DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(), 
    [Deleted] DATETIMEOFFSET NULL, 
    CONSTRAINT [PK_MemberProject_Id] PRIMARY KEY ([MemberId] ASC, [ProjectId] ASC),
    CONSTRAINT [FK_MemberProject_User] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id]),
	CONSTRAINT [FK_MemberProject_Project] FOREIGN KEY ([ProjectId]) REFERENCES [Project]([Id])
)
