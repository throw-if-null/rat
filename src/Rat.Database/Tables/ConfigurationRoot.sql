CREATE TABLE [dbo].[ConfigurationRoot]
(
	[Id] INT NOT NULL IDENTITY(1, 1),
	[Name] NVARCHAR(128) NOT NULL,
	[ConfigurationTypeId] INT NOT NULL,
    [ProjectId] INT NOT NULL,

    [Created] DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    [Modified] DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] INT NOT NULL,
	[ModifiedBy] INT NOT NULL,

	[ValidFrom] datetime2 (0) GENERATED ALWAYS AS ROW START,
	[ValidTo] datetime2 (0) GENERATED ALWAYS AS ROW END,
	PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo),

    CONSTRAINT [PK_ConfigurationRoot_Id] PRIMARY KEY ([Id] ASC),
    CONSTRAINT [FK_ConfigurationRoot_ProjectId__Project_Id] FOREIGN KEY ([ProjectId]) REFERENCES [Project]([Id]),
    CONSTRAINT [FK_ConfigurationRoot_ConfigurationTypeId__ConfigurationType_Id] FOREIGN KEY ([ConfigurationTypeId]) REFERENCES [ConfigurationType]([Id])
)
WITH
(
	SYSTEM_VERSIONING = ON
    (
        HISTORY_TABLE = [dbo].[ConfigurationRootHistory],
        HISTORY_RETENTION_PERIOD = 6 MONTHS
    )
)
