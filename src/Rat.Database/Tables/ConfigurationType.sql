CREATE TABLE [dbo].[ConfigurationType]
(
	[Id] INT NOT NULL IDENTITY(1, 1),
	[Name] NVARCHAR(64) NOT NULL,

	CONSTRAINT [PK_ConfigurationType_Id] PRIMARY KEY ([Id] ASC)
)
