using RecordLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordLibrary
{
    public static class LibraryExtensions
    {
        

        public static decimal RequestSalary(this string message)
        {
            Console.Write(message);
            decimal output = 0;
            while (!decimal.TryParse(Console.ReadLine(), out output))
            {
                Console.Write(message);
            }
            return output;
        }

        public static int RequestInt(this string message)
        {
            Console.Write(message);
            int output = 0;
            while (!int.TryParse(Console.ReadLine(), out output))
            {
                Console.Write(message);
            }
            return output;
        }

        public static string RequesFulltEmployeeInfo(this FullEmployeeModel employee)
        {
            string? output = string.Empty;
            output =
                $"Id:               {employee.BasicInfo.Id}\n"+
                $"Firstname:        {employee.BasicInfo.FirstName}\n" +
                $"Lastname:         {employee.BasicInfo.LastName}\n" +
                $"Current Salary:   {employee.Salary:C2}\n" +
                $"Hiring Date:      {employee.DateHired:d}\n" +
                $"Email Addresses:  \n{GetEmployeeEmails(employee)}\n" +
                $"Phone Numbers:    \n{GetEmployeePhoneNumbers(employee)}";
            return output;
        }

        private static string GetEmployeeEmails(FullEmployeeModel employee)
        {
            string output = string.Empty;
            foreach(var email in employee.EmailAddresses)
            {
                output += $"    {email.EmailAddress}\n";
            }
            return output;
        }

        private static string GetEmployeePhoneNumbers(FullEmployeeModel employee)
        {
            string output = string.Empty;
            foreach (var phone in employee.PhoneNumbers)
            {
                output += $"    {phone.PhoneNumber}\n";
            }
            return output;
        }

        public static void PromoteEmployee(this FullEmployeeModel employee,bool isPromotion, decimal salaryChangeAmount = 1000)
        {
            if(isPromotion)
            {
                employee.Salary += salaryChangeAmount;
                return;
            }
            employee.Salary -= salaryChangeAmount;
        }
    }
}

