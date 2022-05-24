CREATE TABLE [dbo].[ConfigurationType]
(
	[Id] INT NOT NULL IDENTITY(1, 1),
	[Name] NVARCHAR(64) NOT NULL,

	[Timestamp] DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
	[Operation] NVARCHAR(16) DEFAULT N'insert',
	[Operator] INT NOT NULL,

	[ValidFrom] datetime2 (0) GENERATED ALWAYS AS ROW START,
	[ValidTo] datetime2 (0) GENERATED ALWAYS AS ROW END,
	PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo),

	CONSTRAINT [PK_ConfigurationType_Id] PRIMARY KEY ([Id] ASC),
	CONSTRAINT [FK_ConfigurationType_Member_Operator] FOREIGN KEY ([Operator]) REFERENCES [Member]([Id]),
	CONSTRAINT [CH_ConfigurationType_Operation] CHECK ([Operation] IN (N'insert', N'update', N'delete'))
)
WITH
(
	SYSTEM_VERSIONING = ON
    (
        HISTORY_TABLE = [dbo].[ConfigurationTypeHistory],
        HISTORY_RETENTION_PERIOD = 6 MONTHS
    )
)