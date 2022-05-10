CREATE TABLE [dbo].[ProjectType]
(
	[Id] INT NOT NULL IDENTITY(1, 1),
	[Name] NVARCHAR(64) NOT NULL UNIQUE,

	[Timestamp] DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	[Operation] NVARCHAR(16) DEFAULT N'created',
	[Operator] INT NOT NULL,

	[ValidFrom] datetime2 (0) GENERATED ALWAYS AS ROW START NOT NULL,
	[ValidTo] datetime2 (0) GENERATED ALWAYS AS ROW END NOT NULL,
	PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo),

    CONSTRAINT [PK_ProjectType_Id] PRIMARY KEY ([Id] ASC),
	CONSTRAINT [FK_ProjectType_Member_Operator] FOREIGN KEY ([Operator]) REFERENCES [Member]([Id]),
	CONSTRAINT [CH_ProjectType_Operation] CHECK ([Operation] IN (N'insert', N'update', N'delete'))
)
WITH
(
	SYSTEM_VERSIONING = ON
    (
        HISTORY_TABLE = [dbo].[ProjectTypeHistory],
        HISTORY_RETENTION_PERIOD = 6 MONTHS
    )
)
