﻿CREATE TABLE [dbo].[ConfigurationType]
(
	[Id] INT NOT NULL IDENTITY(1, 1),
	[Name] NVARCHAR(64) NOT NULL,

	[Created] DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    [Modified] DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] INT NOT NULL,
	[ModifiedBy] INT NOT NULL,

	[ValidFrom] datetime2 (0) GENERATED ALWAYS AS ROW START,
	[ValidTo] datetime2 (0) GENERATED ALWAYS AS ROW END,
	PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo),

	CONSTRAINT [PK_ConfigurationType_Id] PRIMARY KEY ([Id] ASC)
)
WITH
(
	SYSTEM_VERSIONING = ON
    (
        HISTORY_TABLE = [dbo].[ConfigurationTypeHistory],
        HISTORY_RETENTION_PERIOD = 6 MONTHS
    )
)