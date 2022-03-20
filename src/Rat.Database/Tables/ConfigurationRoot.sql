CREATE TABLE [dbo].[ConfigurationRoot]
(
	[Id] INT NOT NULL IDENTITY(1, 1),
	[Name] NVARCHAR(128) NOT NULL,
	[ConfigurationTypeId] INT NOT NULL,

    CONSTRAINT [PK_ConfigurationRoot_Id] PRIMARY KEY ([Id] ASC),
    CONSTRAINT [FK_ConfigurationRoot_ConfigurationType] FOREIGN KEY ([ConfigurationTypeId]) REFERENCES [ConfigurationType]([Id])
)
