CREATE TABLE [dbo].[EmailAddresses]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [EmailAddress] NVARCHAR(100) NOT NULL,
    [DateCreated] DATETIME2 NOT NULL DEFAULT GetDate(), 
    [DateUpdated] DATETIME2 NOT NULL DEFAULT GetDate()
)
