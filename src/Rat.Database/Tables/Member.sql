CREATE TABLE [dbo].[Member]
(
	[Id] INT NOT NULL IDENTITY(1, 1),
	[AuthProviderId] NVARCHAR(128) NOT NULL,
	[Deleted] BIT NOT NULL DEFAULT 0,

    [Timestamp] DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	[Operation] NVARCHAR(16) DEFAULT N'created',
	[Operator] INT NOT NULL,

	[ValidFrom] datetime2 (0) GENERATED ALWAYS AS ROW START,
	[ValidTo] datetime2 (0) GENERATED ALWAYS AS ROW END,
	PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo),

    CONSTRAINT [PK_User_Id] PRIMARY KEY ([Id] ASC),
	CONSTRAINT [UQ_AuthProviderId] UNIQUE ([AuthProviderId]),
	CONSTRAINT [FK_Member_Member_Operator] FOREIGN KEY ([Operator]) REFERENCES [Member]([Id]),
	CONSTRAINT [CH_Member_Operation] CHECK ([Operation] IN (N'insert', N'update', N'delete'))
)
WITH
(
	SYSTEM_VERSIONING = ON
    (
        HISTORY_TABLE = [dbo].[MemberHistory],
        HISTORY_RETENTION_PERIOD = 6 MONTHS
    )
)