CREATE TABLE [dbo].[EmployeePhoneNumber]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[EmployeeId] INT NOT NULL,
	[PhoneNumberId] INT NOT NULL,
	[DateCreated] DATETIME2 NOT NULL DEFAULT GetDate(), 
    [DateUpdated] DATETIME2 NOT NULL DEFAULT GetDate(), 
    CONSTRAINT [FK_EmployeePhoneNumber_ToEmployees] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees]([Id]), 
	CONSTRAINT [FK_EmployeePhoneNumber_ToPhoneNumbers] FOREIGN KEY ([PhoneNumberId]) REFERENCES [PhoneNumbers]([Id])
)
