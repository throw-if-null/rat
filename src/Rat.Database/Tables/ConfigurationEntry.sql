CREATE TABLE [dbo].[ConfigurationEntry]
(
	[Id] INT NOT NULL IDENTITY(1, 1),
	[ConfigurationRootId] INT NOT NULL,
	[Key] NVARCHAR(128) NOT NULL,
	[Value] NVARCHAR(2048) NOT NULL,
	[Disabled] BIT NOT NULL DEFAULT 0,

	[Created] DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(), 
    [Modified] DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(), 
    CONSTRAINT [PK_ConfigurationEntry_Id] PRIMARY KEY ([Id] ASC),
    CONSTRAINT [FK_ConfigurationEntry_ConfigurationRoot] FOREIGN KEY ([ConfigurationRootId]) REFERENCES [ConfigurationRoot]([Id])
)
