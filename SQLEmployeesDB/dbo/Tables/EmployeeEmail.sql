CREATE TABLE [dbo].[EmployeeEmail]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[EmployeeId] INT NOT NULL,
	[EmailAddressId] INT NOT NULL, 
	[DateCreated] DATETIME2 NOT NULL DEFAULT GetDate(), 
    [DateUpdated] DATETIME2 NOT NULL DEFAULT GetDate(), 
    CONSTRAINT [FK_EmployeeEmail_ToEmployee] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees]([Id]),
	CONSTRAINT [FK_EmployeeEmail_ToEmailAddress] FOREIGN KEY ([EmailAddressId]) REFERENCES [EmailAddresses]([Id])
)
