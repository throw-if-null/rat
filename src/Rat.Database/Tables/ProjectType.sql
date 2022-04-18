﻿CREATE TABLE [dbo].[ProjectType]
(
	[Id] INT NOT NULL IDENTITY(1, 1),
	[Name] NVARCHAR(64) NOT NULL UNIQUE,

    CONSTRAINT [PK_ProjectType_Id] PRIMARY KEY ([Id] ASC)
)