﻿CREATE TABLE [dbo].[Project]
(
	[Id] INT NOT NULL IDENTITY(1, 1),
	[Name] NVARCHAR(128) NOT NULL,
	[ProjectTypeId] INT NOT NULL,

	[Created] DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    [Modified] DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] INT NOT NULL,
	[ModifiedBy] INT NOT NULL,

	[ValidFrom] datetime2 (0) GENERATED ALWAYS AS ROW START,
	[ValidTo] datetime2 (0) GENERATED ALWAYS AS ROW END,
	PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo),

    CONSTRAINT [PK_Project_Id] PRIMARY KEY ([Id] ASC),
    CONSTRAINT [FK_Project_ProjectType] FOREIGN KEY ([ProjectTypeId]) REFERENCES [ProjectType]([Id])
)
WITH
(
	SYSTEM_VERSIONING = ON
    (
        HISTORY_TABLE = [dbo].[ProjectHistory],
        HISTORY_RETENTION_PERIOD = 6 MONTHS
    )
)