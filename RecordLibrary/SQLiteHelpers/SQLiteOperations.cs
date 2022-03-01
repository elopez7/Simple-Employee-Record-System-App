using RecordLibrary.BaseClasses;
using RecordLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordLibrary.SQLiteHelpers
{
    public class SQLiteOperations : DatabaseOperations
    {
        public SQLiteOperations(string connectionString)
        {
            _connectionString = connectionString;
            db = new SQLiteDataAccess();
        }

        protected override void SaveEmployeeBasicInformation(FullEmployeeModel employee)
        {
            string sql = "insert into Employees (FirstName, LastName, DateHired) values (@FirstName, @LastName, @DateHired)";
            db.SaveData(sql,
                        new { employee.BasicInfo.FirstName, employee.BasicInfo.LastName, employee.DateHired },
                        _connectionString);
        }

        protected override int GetEmployeeIdByNameAndLastName(FullEmployeeModel employee)
        {
            string sql = "select Id from Employees where FirstName = @FirstName and LastName = @LastName";
            int employeeId = db.LoadData<IdLookupModel, dynamic>(sql,
                            new { employee.BasicInfo.FirstName, employee.BasicInfo.LastName },
                            _connectionString).First().Id;

            return employeeId;
        }

        protected override void SaveEmployeePhoneNumbers(FullEmployeeModel employee, int employeeId)
        {
            string sql = string.Empty;
            foreach (var phoneNumber in employee.PhoneNumbers)
            {
                if (phoneNumber.Id == 0)
                {
                    sql = "insert into PhoneNumbers (PhoneNumber) values (@PhoneNumber)";
                    db.SaveData(sql, new { phoneNumber.PhoneNumber }, _connectionString);

                    sql = "select Id from PhoneNumbers where PhoneNumber = @PhoneNumber;";
                    phoneNumber.Id = db.LoadData<IdLookupModel, dynamic>(sql,
                        new { phoneNumber.PhoneNumber },
                        _connectionString).First().Id;
                }

                sql = "insert into EmployeePhoneNumber (EmployeeId, PhoneNumberId) values (@EmployeeId, @PhoneNumberId);";
                db.SaveData(sql, new { EmployeeId = employeeId, PhoneNumberId = phoneNumber.Id }, _connectionString);
            }
        }

        protected override void SaveEmployeeEmailAddresses(FullEmployeeModel employee, int employeeId)
        {
            string sql = string.Empty;
            foreach (var email in employee.EmailAddresses)
            {
                if (email.Id == 0)
                {
                    sql = "insert into EmailAddresses (EmailAddress) values (@EmailAddress)";
                    db.SaveData(sql, new { email.EmailAddress }, _connectionString);

                    sql = "select Id from EmailAddresses where EmailAddress = @EmailAddress;";
                    email.Id = db.LoadData<IdLookupModel, dynamic>(sql,
                        new { email.EmailAddress },
                        _connectionString).First().Id;
                }

                sql = "insert into EmployeeEmail (EmployeeId, EmailAddressId) values (@EmployeeId, @EmailAddressId);";
                db.SaveData(sql, new { EmployeeId = employeeId, EmailAddressId = email.Id }, _connectionString);
            }
        }

        protected override List<BasicEmployeeModel> GetAllEmployeesBasicInformation()
        {
            string sql = "select Id, FirstName, LastName from Employees;";
            return db.LoadData<BasicEmployeeModel, dynamic>(sql, new { }, _connectionString);
        }

        protected override BasicEmployeeModel GetBasicEmployeeById(int idNumber)
        {
            BasicEmployeeModel output = new();
            string sql = "select Id, FirstName, LastName from Employees where Id = @Id";

            return db.LoadData<BasicEmployeeModel, dynamic>(sql,
                new { Id = idNumber },
                _connectionString).FirstOrDefault();
        }

        protected override void UpdateEmployeeStatusById(int idNumber)
        {
            FullEmployeeModel employee = GetFullEmployeeById(idNumber);
            string sql = "update Employees set IsActive = @IsActive where Id = @Id";
            db.SaveData(sql, new { Id = idNumber, IsActive = false }, _connectionString);
        }

        protected override void UpdateEmployeeSalaryById(int idNumber, bool isPromotion = true)
        {
            FullEmployeeModel employee = GetFullEmployeeById(idNumber);

            decimal newSalary = CalculateNewSalary(employee.Salary, isPromotion);
            string sql = "update Employees set Salary = @Salary where Id = @Id";
            db.SaveData(sql, new { Id = idNumber, Salary = newSalary }, _connectionString);
        }

        protected override decimal CalculateNewSalary(decimal currentSalary, bool isPromotion)
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

        protected override decimal GetEmployeeSalaryById(int idNumber)
        {
            string sql = "select Salary from Employees where Id = @Id";

            return db.LoadData<FullEmployeeModel, dynamic>(sql,
                            new { Id = idNumber },
                            _connectionString).FirstOrDefault().Salary;
        }

        protected override bool GetEmployeeCurrentStatus(int idNumber)
        {
            string sql = "select IsActive from Employees where Id = @Id";

            return db.LoadData<FullEmployeeModel, dynamic>(sql,
                            new { Id = idNumber },
                            _connectionString).FirstOrDefault().IsActive;

        }

        protected override List<EmailAddressModel> GetEmployeeEmailAddressesById(int employeeId)
        {
            string sql = @"select e.*
                    from EmailAddresses e
                    inner join
                    EmployeeEmail ee on ee.EmailAddressId = e.Id
                    where ee.EmployeeId = @Id";

            return db.LoadData<EmailAddressModel, dynamic>(sql, new { Id = employeeId }, _connectionString);
        }

        protected override List<PhoneNumberModel> GetEmployeePhoneNumbersById(int employeeId)
        {
            string sql = @"select p.*
                    from PhoneNumbers p
                    inner
                    join EmployeePhoneNumber ep on ep.PhoneNumberId = p.Id
                    where ep.EmployeeId = @Id";

            return db.LoadData<PhoneNumberModel, dynamic>(sql, new { Id = employeeId }, _connectionString);
        }
    }
}
