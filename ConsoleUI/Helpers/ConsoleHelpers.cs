using RecordLibrary.SQLHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI.Helpers
{
    public static class ConsoleHelpers
    {
        private static DatabaseOperations operations = new DatabaseOperations(DatabaseHelper.GetConnectionString());


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
    }
}
