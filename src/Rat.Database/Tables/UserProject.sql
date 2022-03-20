CREATE TABLE [dbo].[UserProject]
(
	[UserId] INT NOT NULL,
	[ProjectId] INT NOT NULL,

	CONSTRAINT [PK_UserProject_Id] PRIMARY KEY ([UserId] ASC, [ProjectId] ASC),
    CONSTRAINT [FK_UserProject_User] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]),
	CONSTRAINT [FK_UserProject_Project] FOREIGN KEY ([ProjectId]) REFERENCES [Project]([Id])
)
