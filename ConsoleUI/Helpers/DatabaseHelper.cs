using ConsoleUI.IO;
using RecordLibrary;
using RecordLibrary.Models;
using RecordLibrary.BaseClasses;

namespace ConsoleUI.Helpers
{
    public static class DatabaseHelper
    {
        public static void CreateFakeEmployee(DatabaseOperations dbOperations)
        {
            FullEmployeeModel employee = new()
            {
                BasicInfo = new()
                {
                    Id = 1,
                    FirstName = Faker.Name.First(),
                    LastName = Faker.Name.Last()
                }

            };

            employee.EmailAddresses.Add(new EmailAddressModel { EmailAddress = Faker.Internet.Email(employee.BasicInfo.FirstName) });
            employee.EmailAddresses.Add(new EmailAddressModel { EmailAddress = Faker.Internet.Email(employee.BasicInfo.LastName) });

            employee.PhoneNumbers.Add(new PhoneNumberModel { PhoneNumber = Faker.Phone.Number() });
            employee.PhoneNumbers.Add(new PhoneNumberModel { PhoneNumber = Faker.Phone.Number() });
            employee.PhoneNumbers.Add(new PhoneNumberModel { PhoneNumber = Faker.Phone.Number() });


            employee.DateHired = DateTime.Now;

            dbOperations.AddEmployeeToDatabase(employee);

        }

        public static void CreateNewEmployee(DatabaseOperations dbOperations)
        {
            FullEmployeeModel employee = new()
            {
                BasicInfo = new()
                {
                    Id = 1,
                    FirstName = "Enter Firstname: ".RequestString(),
                    LastName = "Enter Lastname: ".RequestString()
                }

            };

            employee.EmailAddresses.Add(new EmailAddressModel { EmailAddress = "Enter Email Address: ".RequestString() });

            employee.PhoneNumbers.Add(new PhoneNumberModel { PhoneNumber = "Enter Phone Number: ".RequestString() });

            employee.DateHired = DateTime.Now;

            dbOperations.AddEmployeeToDatabase(employee);
        }

        public static void FireEmployee(DatabaseOperations dbOperations)
        {
            dbOperations.FireEmployee("Enter Employee Id: ".RequestInteger());
        }

        public static void GetAllEmployess(DatabaseOperations dbOperations)
        {
            var employees = dbOperations.GetAllEmployees();
            UserInterface.DisplayListHeader(ConsoleColor.Green, "ALL");
            foreach (var employee in employees)
            {
                PrintEmployeeInformation(employee);
            }
        }

        public static void GetActiveEmployees(DatabaseOperations dbOperations)
        {
            var employees = dbOperations.GetActiveEmployees();
            UserInterface.DisplayListHeader(ConsoleColor.Cyan, "CURRENT");
            foreach(var employee in employees)
            {
                PrintEmployeeInformation(employee);
            }
        }

        public static void GetFormerEmployees(DatabaseOperations dbOperations)
        {
            var employees = dbOperations.GetFormerEmployees();
            UserInterface.DisplayListHeader(ConsoleColor.Cyan, "FORMER");
            foreach (var employee in employees)
            {
                PrintEmployeeInformation(employee);
            }
        }

        public static void GetFullEmployeeInformationById(DatabaseOperations dbOperations)
        {
            var employee = dbOperations.GetFullEmployeeById("Enter Employee Id: ".RequestInteger());
            PrintEmployeeInformation(employee);
        }

        public static void PromoteEmployee(DatabaseOperations dbOperations)
        {
            dbOperations.PromoteEmployee("Enter Employee Id: ".RequestInteger());
        }

        public static void DemoteEmployee(DatabaseOperations dbOperations)
        {
            dbOperations.DemoteEmployee("Enter Employee Id: ".RequestInteger());
        }

        private static void PrintEmployeeInformation(FullEmployeeModel employee)
        {
            Console.WriteLine("=====================================\n");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"{employee.RequesFulltEmployeeInfo()}");
            Console.ResetColor();
            Console.WriteLine("=====================================");
        }
    }
}
