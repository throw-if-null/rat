CREATE TABLE [dbo].[User]
(
	[Id] INT NOT NULL IDENTITY(1, 1),
	[AuthProviderUserId] NVARCHAR(128) NOT NULL,

    CONSTRAINT [PK_User_Id] PRIMARY KEY ([Id] ASC)
)

GO

CREATE INDEX [IX_User_UserId] ON [dbo].[User] ([AuthProviderUserId])
