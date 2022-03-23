CREATE TABLE [dbo].[MemberProject]
(
	[MemberId] INT NOT NULL,
	[ProjectId] INT NOT NULL,

	CONSTRAINT [PK_MemberProject_Id] PRIMARY KEY ([MemberId] ASC, [ProjectId] ASC),
    CONSTRAINT [FK_MemberProject_User] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id]),
	CONSTRAINT [FK_MemberProject_Project] FOREIGN KEY ([ProjectId]) REFERENCES [Project]([Id])
)
