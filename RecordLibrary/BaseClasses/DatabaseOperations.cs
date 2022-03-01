using RecordLibrary.Models;
using RecordLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RecordLibrary.BaseClasses
{
    public abstract class DatabaseOperations
    {
        protected string? _connectionString;
        protected IDataAccess? db;

        public List<FullEmployeeModel> GetAllEmployees()
        {
            List<FullEmployeeModel> output = new();
            List<BasicEmployeeModel> employeesBasicInfo = GetAllEmployeesBasicInformation();

            foreach (var employee in employeesBasicInfo)
            {
                output.Add(GetEmployeeFromDatabase(employee));
            }
            return output;
        }

        public List<FullEmployeeModel> GetActiveEmployees()
        {
            var employees = GetAllEmployees();
            var output = employees.Where(a => a.IsActive == true).ToList();
            return output;
        }

        public List<FullEmployeeModel> GetFormerEmployees()
        {
            var employees = GetAllEmployees();
            var output = employees.Where(a => a.IsActive == false).ToList();
            return output;
        }

        public FullEmployeeModel GetFullEmployeeById(int idNumber)
        {
            FullEmployeeModel output = new();
            output.BasicInfo = GetBasicEmployeeById(idNumber);

            while (output.BasicInfo == null)
            {
                idNumber = "Id not found. Please try again: ".RequestInt();
                output.BasicInfo = GetBasicEmployeeById(idNumber);
            }

            output.Salary = GetEmployeeSalaryById(idNumber);
            output.EmailAddresses = GetEmployeeEmailAddressesById(idNumber);
            output.PhoneNumbers = GetEmployeePhoneNumbersById(idNumber);

            return output;
        }

        public void FireEmployee(int idNumber)
        {
            UpdateEmployeeStatusById(idNumber);
        }

        public void PromoteEmployee(int idNumber)
        {
            UpdateEmployeeSalaryById(idNumber);
        }

        public void DemoteEmployee(int idNumber)
        {
            UpdateEmployeeSalaryById(idNumber, false);
        }

        public void AddEmployeeToDatabase(FullEmployeeModel employee)
        {
            SaveEmployeeBasicInformation(employee);
            int employeeId = GetEmployeeIdByNameAndLastName(employee);
            SaveEmployeePhoneNumbers(employee, employeeId);
            SaveEmployeeEmailAddresses(employee, employeeId);
        }

        private FullEmployeeModel GetEmployeeFromDatabase(BasicEmployeeModel employee)
        {
            FullEmployeeModel output = new();
            output.BasicInfo = employee;
            output.EmailAddresses = GetEmployeeEmailAddressesById(output.BasicInfo.Id);
            output.PhoneNumbers = GetEmployeePhoneNumbersById(output.BasicInfo.Id);
            output.IsActive = GetEmployeeCurrentStatus(output.BasicInfo.Id);
            output.Salary = GetEmployeeSalaryById(output.BasicInfo.Id);
            return output;
        }

        protected abstract void SaveEmployeeBasicInformation(FullEmployeeModel employee);
        protected abstract int GetEmployeeIdByNameAndLastName(FullEmployeeModel employee);
        protected abstract void SaveEmployeePhoneNumbers(FullEmployeeModel employee, int employeeId);
        protected abstract void SaveEmployeeEmailAddresses(FullEmployeeModel employee, int employeeId);
        protected abstract List<BasicEmployeeModel> GetAllEmployeesBasicInformation();
        protected abstract BasicEmployeeModel GetBasicEmployeeById(int idNumber);
        protected abstract void UpdateEmployeeStatusById(int idNumber);
        protected abstract void UpdateEmployeeSalaryById(int idNumber, bool isPromotion = true);
        protected abstract decimal CalculateNewSalary(decimal currentSalary, bool isPromotion);
        protected abstract decimal GetEmployeeSalaryById(int idNumber);
        protected abstract bool GetEmployeeCurrentStatus(int idNumber);
        protected abstract List<EmailAddressModel> GetEmployeeEmailAddressesById(int employeeId);
        protected abstract List<PhoneNumberModel> GetEmployeePhoneNumbersById(int employeeId);
    }
}
