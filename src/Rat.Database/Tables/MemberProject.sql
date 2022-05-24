CREATE TABLE [dbo].[MemberProject]
(
	[MemberId] INT NOT NULL,
	[ProjectId] INT NOT NULL,

	[Timestamp] DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	[Operation] NVARCHAR(16) DEFAULT N'insert',
	[Operator] INT NOT NULL,

	[ValidFrom] datetime2 (0) GENERATED ALWAYS AS ROW START,
	[ValidTo] datetime2 (0) GENERATED ALWAYS AS ROW END,
	PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo),

    CONSTRAINT [PK_MemberProject_Id] PRIMARY KEY ([MemberId] ASC, [ProjectId] ASC),
    CONSTRAINT [FK_MemberProject_User] FOREIGN KEY ([MemberId]) REFERENCES [Member]([Id]),
	CONSTRAINT [FK_MemberProject_Project] FOREIGN KEY ([ProjectId]) REFERENCES [Project]([Id]),
	CONSTRAINT [FK_MemberProject_Member_Operator] FOREIGN KEY ([Operator]) REFERENCES [Member]([Id]),
	CONSTRAINT [CH_MemberProject_Operation] CHECK ([Operation] IN (N'insert', N'update', N'delete'))
)
WITH
(
	SYSTEM_VERSIONING = ON
    (
        HISTORY_TABLE = [dbo].[MemberProjectHistory],
        HISTORY_RETENTION_PERIOD = 6 MONTHS
    )
)