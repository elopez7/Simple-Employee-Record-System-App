using RecordLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordLibrary.SQLHelpers
{
    public class DatabaseOperations
    {
        private readonly string _connectionString;
        private SQLDataAccess db = new();

        public DatabaseOperations(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<FullEmployeeModel> GetAllFullEmployees()
        {
            List<FullEmployeeModel> output = new();
            List<BasicEmployeeModel> employeesBasicInfo = GetAllEmployeesBasicInformation();

            foreach(var employee in employeesBasicInfo)
            {
                output.Add(GetEmployeeFromDatabase(employee));
            }
            return output;
        }

        public List<FullEmployeeModel> GetActiveEmployees()
        {
            var employees = GetAllFullEmployees();
            var output = employees.Where(a => a.IsActive == true).ToList();
            return output;
        }

        public List<FullEmployeeModel> GetFormerEmployees()
        {
            var employees = GetAllFullEmployees();
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
            string sql = "insert into dbo.Employees (FirstName, LastName, DateHired) values (@FirstName, @LastName, @DateHired)";
            db.SaveData(sql,
                        new { employee.BasicInfo.FirstName, employee.BasicInfo.LastName , employee.DateHired },
                        _connectionString);

            sql = "select Id from dbo.Employees where FirstName = @FirstName and LastName = @LastName";
            int employeeId = db.LoadData<IdLookupModel, dynamic>(sql,
                            new {employee.BasicInfo.FirstName, employee.BasicInfo.LastName},
                            _connectionString).First().Id;

            foreach(var phoneNumber in employee.PhoneNumbers)
            {
                if(phoneNumber.Id == 0)
                {
                    sql = "insert into dbo.PhoneNumbers (PhoneNumber) values (@PhoneNumber)";
                    db.SaveData(sql, new { phoneNumber.PhoneNumber }, _connectionString);

                    sql = "select Id from dbo.PhoneNumbers where PhoneNumber = @PhoneNumber;";
                    phoneNumber.Id = db.LoadData<IdLookupModel, dynamic>(sql,
                        new { phoneNumber.PhoneNumber },
                        _connectionString).First().Id;
                }

                sql = "insert into dbo.EmployeePhoneNumber (EmployeeId, PhoneNumberId) values (@EmployeeId, @PhoneNumberId);";
                db.SaveData(sql, new { EmployeeId = employeeId, PhoneNumberId = phoneNumber.Id }, _connectionString);
            }

            foreach (var email in employee.EmailAddresses)
            {
                if (email.Id == 0)
                {
                    sql = "insert into dbo.EmailAddresses (EmailAddress) values (@EmailAddress)";
                    db.SaveData(sql, new { email.EmailAddress }, _connectionString);

                    sql = "select Id from dbo.EmailAddresses where EmailAddress = @EmailAddress;";
                    email.Id = db.LoadData<IdLookupModel, dynamic>(sql,
                        new { email.EmailAddress },
                        _connectionString).First().Id;
                }

                sql = "insert into dbo.EmployeeEmail (EmployeeId, EmailAddressId) values (@EmployeeId, @EmailAddressId);";
                db.SaveData(sql, new { EmployeeId = employeeId, EmailAddressId = email.Id }, _connectionString);
            }
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

        private List<BasicEmployeeModel> GetAllEmployeesBasicInformation()
        {
            string sql = "select Id, FirstName, LastName from dbo.Employees;";
            return db.LoadData<BasicEmployeeModel, dynamic>(sql, new { }, _connectionString);
        }

        private BasicEmployeeModel GetBasicEmployeeById(int idNumber)
        {
            BasicEmployeeModel output = new();
            string sql = "select Id, FirstName, LastName from dbo.Employees where Id = @Id";

            return db.LoadData<BasicEmployeeModel, dynamic>(sql,
                new { Id = idNumber },
                _connectionString).FirstOrDefault();
        }

        private void UpdateEmployeeStatusById(int idNumber)
        {
            FullEmployeeModel employee = GetFullEmployeeById(idNumber);
            string sql = "update dbo.Employees set IsActive = @IsActive where Id = @Id";
            db.SaveData(sql, new { Id = idNumber, IsActive = false }, _connectionString);
        }

        private void UpdateEmployeeSalaryById(int idNumber, bool isPromotion = true)
        {
            FullEmployeeModel employee = GetFullEmployeeById(idNumber);
            
            decimal  newSalary = CalculateNewSalary(employee.Salary, isPromotion);
            string sql = "update dbo.Employees set Salary = @Salary where Id = @Id";
            db.SaveData(sql, new { Id = idNumber, Salary = newSalary }, _connectionString);
        }

        private decimal CalculateNewSalary(decimal currentSalary, bool isPromotion)
        {
            decimal raiseAmount = 0;
            if (isPromotion)
            {
                raiseAmount = "Enter the amount to raise salary by: ".RequestSalary();
                return (currentSalary + raiseAmount);
            }
            raiseAmount = "Enter the amount to lower salary by: ".RequestSalary();
            return (currentSalary - raiseAmount);
        }

        private decimal GetEmployeeSalaryById(int idNumber)
        {
            string sql = "select Salary from dbo.Employees where Id = @Id";
            
            return db.LoadData<FullEmployeeModel, dynamic>(sql,
                            new { Id = idNumber },
                            _connectionString).FirstOrDefault().Salary;
        }

        private bool GetEmployeeCurrentStatus(int idNumber)
        {
            string sql = "select IsActive from dbo.Employees where Id = @Id";

            return db.LoadData<FullEmployeeModel, dynamic>(sql,
                            new { Id = idNumber },
                            _connectionString).FirstOrDefault().IsActive;

        }

        private List<EmailAddressModel> GetEmployeeEmailAddressesById(int employeeId)
        {
            string sql = @"select e.*
                    from dbo.EmailAddresses e
                    inner join
                    dbo.EmployeeEmail ee on ee.EmailAddressId = e.Id
                    where ee.EmployeeId = @Id";

            return db.LoadData<EmailAddressModel, dynamic>(sql, new { Id = employeeId }, _connectionString);
        }

        private List<PhoneNumberModel> GetEmployeePhoneNumbersById(int employeeId)
        {
            string sql = @"select p.*
                    from dbo.PhoneNumbers p
                    inner
                    join dbo.EmployeePhoneNumber ep on ep.PhoneNumberId = p.Id
                    where ep.EmployeeId = @Id";

            return db.LoadData<PhoneNumberModel, dynamic>(sql, new { Id = employeeId }, _connectionString);
        }
    }
}
