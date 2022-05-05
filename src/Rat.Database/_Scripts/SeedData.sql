/*
Post-Deployment Script Template
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.
 Use SQLCMD syntax to include a file in the post-deployment script.
 Example:      :r .\myfile.sql
 Use SQLCMD syntax to reference a variable in the post-deployment script.
 Example:      :setvar TableName MyTable
               SELECT * FROM [$(TableName)]
--------------------------------------------------------------------------------------
*/

-- Insert machine user
SET IDENTITY_INSERT [dbo].[Member] ON

INSERT INTO [dbo].[Member] ([Id], [AuthProviderId], [Created], [Modified], [CreatedBy], [ModifiedBy])
SELECT 1, '', GETUTCDATE(), GETUTCDATE(), 1, 1
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Member] WHERE Id = 1)

SET IDENTITY_INSERT [dbo].[Member] OFF

-- Insert project types
SET IDENTITY_INSERT [dbo].[ProjectType] ON

INSERT INTO [dbo].[ProjectType] ([Id], [Name], [Created], [Modified], [CreatedBy], [ModifiedBy])
SELECT 1, 'js', GETUTCDATE(), GETUTCDATE(), 1, 1
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[ProjectType] WHERE Id = 1)

INSERT INTO [dbo].[ProjectType] ([Id], [Name], [Created], [Modified], [CreatedBy], [ModifiedBy])
SELECT 2, 'csharp', GETUTCDATE(), GETUTCDATE(), 1, 1
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[ProjectType] WHERE Id = 2)

SET IDENTITY_INSERT [dbo].[ProjectType] OFF

-- Insert configuration types
SET IDENTITY_INSERT [dbo].[ConfigurationType] ON

INSERT INTO [dbo].[ConfigurationType] ([Id], [Name], [Created], [Modified], [CreatedBy], [ModifiedBy])
SELECT 1, 'Web', GETUTCDATE(), GETUTCDATE(), 1, 1
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationType] WHERE Id = 1)

INSERT INTO [dbo].[ConfigurationType] ([Id], [Name], [Created], [Modified], [CreatedBy], [ModifiedBy])
SELECT 2, 'Desktop', GETUTCDATE(), GETUTCDATE(), 1, 1
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationType] WHERE Id = 2)

INSERT INTO [dbo].[ConfigurationType] ([Id], [Name], [Created], [Modified], [CreatedBy], [ModifiedBy])
SELECT 3, 'Mobile', GETUTCDATE(), GETUTCDATE(), 1, 1
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationType] WHERE Id = 3)

INSERT INTO [dbo].[ConfigurationType] ([Id], [Name], [Created], [Modified], [CreatedBy], [ModifiedBy])
SELECT 4, 'Backend', GETUTCDATE(), GETUTCDATE(), 1, 1
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[ConfigurationType] WHERE Id = 4)

SET IDENTITY_INSERT [dbo].[ConfigurationType] OFF