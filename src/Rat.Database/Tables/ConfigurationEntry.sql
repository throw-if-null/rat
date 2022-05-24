CREATE TABLE [dbo].[ConfigurationEntry]
(
	[Id] INT NOT NULL IDENTITY(1, 1),
	[ConfigurationRootId] INT NOT NULL,
	[Key] NVARCHAR(128) NOT NULL,
	[Value] NVARCHAR(2048) NOT NULL,
	[SecondsToLive] INT NOT NULL DEFAULT -1,
	[Disabled] BIT NOT NULL DEFAULT 0,

    [Timestamp] DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	[Operation] NVARCHAR(16) DEFAULT N'insert',
	[Operator] INT NOT NULL,

	[ValidFrom] datetime2 (0) GENERATED ALWAYS AS ROW START,
	[ValidTo] datetime2 (0) GENERATED ALWAYS AS ROW END,
	PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo),

    CONSTRAINT [PK_ConfigurationEntry_Id] PRIMARY KEY ([Id] ASC),
    CONSTRAINT [FK_ConfigurationEntry_ConfigurationRootId-ConfigurationRoot_Id] FOREIGN KEY ([ConfigurationRootId]) REFERENCES [ConfigurationRoot]([Id]),
	CONSTRAINT [FK_ConfigurationEntry_Member_Operator] FOREIGN KEY ([Operator]) REFERENCES [Member]([Id]),
	CONSTRAINT [CH_ConfigurationEntry_Operation] CHECK ([Operation] IN (N'insert', N'update', N'delete'))
)
WITH
(
	SYSTEM_VERSIONING = ON
    (
        HISTORY_TABLE = [dbo].[ConfigurationEntryHistory],
        HISTORY_RETENTION_PERIOD = 6 MONTHS
    )
)