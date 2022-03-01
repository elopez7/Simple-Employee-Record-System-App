using RecordLibrary.BaseClasses;
using RecordLibrary.SQLHelpers;
using RecordLibrary.SQLiteHelpers;
using Microsoft.Extensions.Configuration;

namespace ConsoleUI.Helpers
{
    internal static class ConsoleHelpers
    {
        private static DatabaseOperations? operations; //= new SQLOperations(GetConnectionString("SqlString"));
        

        public static void ChooseQSLServer(string databaseProvider)
        {
            operations = new SQLOperations(GetConnectionString(databaseProvider));
        }

        public static void ChooseQSLite(string databaseProvider)
        {
            operations = new SQLiteOperations(GetConnectionString(databaseProvider));
        }

        public static void InitializeData()
        {
            for(var i = 0; i < 10; i++)
            {
                DatabaseHelper.CreateFakeEmployee(operations);
            }
        }

        public static void HireEmployee()
        {
            DatabaseHelper.CreateNewEmployee(operations);
        }

        public static void GetAllEmployees()
        {
            DatabaseHelper.GetAllEmployess(operations);
        }

        public static void GetCurrentEmployees()
        {
            DatabaseHelper.GetActiveEmployees(operations);
        }

        public static void GetFormerEmployees()
        {
            DatabaseHelper.GetFormerEmployees(operations);
        }

        public static void FindEmployeeById()
        {

            DatabaseHelper.GetFullEmployeeInformationById(operations);
        }

        public static void FireEmployee()
        {
            DatabaseHelper.FireEmployee(operations);
        }

        public static void PromoteEmployee()
        {
            DatabaseHelper.PromoteEmployee(operations);
        }

        public static void DemoteEmployee()
        {
            DatabaseHelper.DemoteEmployee(operations);
            Console.WriteLine("Demote Operation Success!");
        }

        private static string GetConnectionString(string connectionStringName = "Default")
        {
            string output = string.Empty;

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();

            output = config.GetConnectionString(connectionStringName);

            return output;
        }
    }
}
