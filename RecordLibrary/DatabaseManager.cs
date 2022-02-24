using RecordLibrary.Defaults;
using RecordLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordLibrary
{
    public class DatabaseManager
    {
        private List<FullEmployeeModel> currentEmployees = new();
        private List<FullEmployeeModel> formerEmployees = new();

        public void HireEmployee()
        {
            FullEmployeeModel employee = new FullEmployeeModel();
            //employee.FirstName = "Enter firstname: ".RequestString();
            //employee.LastName = "Enter lasttname: ".RequestString();
            //employee.EmployeeNumber = DefaultGlobalValues.FirstEmployeeNumber++;
            employee.Salary = DefaultGlobalValues.DefaultStartingSalary;
            currentEmployees.Add(employee);   
        }

        public void FireEmployee()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            FullEmployeeModel? firedEmployee = FindEmployeeByNumber();

            if (currentEmployees.Contains(firedEmployee))
            {
                currentEmployees.Remove(firedEmployee);
                formerEmployees.Add(firedEmployee);
                Console.ResetColor();
                return;
            }

            Console.WriteLine("Employee not found");
            Console.ResetColor();
        }

        public void PromoteEmployee()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            FullEmployeeModel? promotedEmployee = FindEmployeeByNumber();

            if (currentEmployees.Contains(promotedEmployee))
            {
                promotedEmployee.PromoteEmployee(true);
                return;
            }
            Console.WriteLine("Employee not found");
            Console.ResetColor();
        }

        public void DemoteEmployee()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            FullEmployeeModel? promotedEmployee = FindEmployeeByNumber();

            if (currentEmployees.Contains(promotedEmployee))
            {
                promotedEmployee.PromoteEmployee(false);
                return;
            }
            Console.WriteLine("Employee not found");
            Console.ResetColor();
        }

        public void ShowAllEmployees()
        {
            List<FullEmployeeModel> allEmployees = new List<FullEmployeeModel>();
            allEmployees.AddRange(currentEmployees);
            allEmployees.AddRange(formerEmployees);

            OutputListInformation(allEmployees, ConsoleColor.Green, "ALL");
        }

        public void ShowCurrentEmployees()
        {
            OutputListInformation(currentEmployees, ConsoleColor.DarkBlue, "CURRENT");
        }

        public void ShowPreviousEmployees()
        {
            OutputListInformation(formerEmployees, ConsoleColor.DarkRed, "PREVIOUS");
        }

        private void OutputListInformation(List<FullEmployeeModel> requestedList, ConsoleColor foregroundColor, string employeeType)
        {
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine($"----{employeeType} EMPLOYEES----");
            Console.ResetColor();
            foreach (FullEmployeeModel employee in requestedList)
            {
                Console.WriteLine("--------");
                //Console.WriteLine(employee.RequesBasictEmployeeInfo());
                Console.WriteLine("--------");
            }
        }

        private FullEmployeeModel? FindEmployeeByNumber()
        {
            int? employeeNumber = "Enter the employee number ".RequestInt();
            FullEmployeeModel? output = new();//currentEmployees.Find(employee => employee.EmployeeNumber == employeeNumber);
            return output;
        }

    }
}
