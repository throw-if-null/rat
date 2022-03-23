CREATE TABLE [dbo].[Member]
(
	[Id] INT NOT NULL IDENTITY(1, 1),
	[AuthProviderId] NVARCHAR(128) NOT NULL,

    CONSTRAINT [PK_User_Id] PRIMARY KEY ([Id] ASC),
	CONSTRAINT [UQ_AuthProviderId] UNIQUE ([AuthProviderId])
)

GO

CREATE INDEX [IX_User_AuthProviderId] ON [dbo].[Member] ([AuthProviderId])
GO
