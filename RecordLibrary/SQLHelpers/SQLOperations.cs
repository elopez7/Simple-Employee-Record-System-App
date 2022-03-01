using RecordLibrary.BaseClasses;
using RecordLibrary.Models;

namespace RecordLibrary.SQLHelpers
{
    public class SQLOperations : DatabaseOperations
    {
        public SQLOperations(string connectionString)
        {
            _connectionString = connectionString;
            db = new SQLDataAccess();
        }

        protected override void SaveEmployeeBasicInformation(FullEmployeeModel employee)
        {
            string sql = "insert into dbo.Employees (FirstName, LastName, DateHired) values (@FirstName, @LastName, @DateHired)";
            db.SaveData(sql,
                        new { employee.BasicInfo.FirstName, employee.BasicInfo.LastName, employee.DateHired },
                        _connectionString);
        }

        protected override int GetEmployeeIdByNameAndLastName(FullEmployeeModel employee)
        {
            string sql = "select Id from dbo.Employees where FirstName = @FirstName and LastName = @LastName";
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
        }

        protected override void SaveEmployeeEmailAddresses(FullEmployeeModel employee, int employeeId)
        {
            string sql = string.Empty;
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

        protected override List<BasicEmployeeModel> GetAllEmployeesBasicInformation()
        {
            string sql = "select Id, FirstName, LastName from dbo.Employees;";
            return db.LoadData<BasicEmployeeModel, dynamic>(sql, new { }, _connectionString);
        }

        protected override BasicEmployeeModel GetBasicEmployeeById(int idNumber)
        {
            BasicEmployeeModel output = new();
            string sql = "select Id, FirstName, LastName from dbo.Employees where Id = @Id";

            return db.LoadData<BasicEmployeeModel, dynamic>(sql,
                new { Id = idNumber },
                _connectionString).FirstOrDefault();
        }

        protected override void UpdateEmployeeStatusById(int idNumber)
        {
            FullEmployeeModel employee = GetFullEmployeeById(idNumber);
            string sql = "update dbo.Employees set IsActive = @IsActive where Id = @Id";
            db.SaveData(sql, new { Id = idNumber, IsActive = false }, _connectionString);
        }

        protected override void UpdateEmployeeSalaryById(int idNumber, bool isPromotion = true)
        {
            FullEmployeeModel employee = GetFullEmployeeById(idNumber);
            
            decimal newSalary = CalculateNewSalary(employee.Salary, isPromotion);
            string sql = "update dbo.Employees set Salary = @Salary where Id = @Id";
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
            string sql = "select Salary from dbo.Employees where Id = @Id";
            
            return db.LoadData<FullEmployeeModel, dynamic>(sql,
                            new { Id = idNumber },
                            _connectionString).FirstOrDefault().Salary;
        }

        protected override bool GetEmployeeCurrentStatus(int idNumber)
        {
            string sql = "select IsActive from dbo.Employees where Id = @Id";

            return db.LoadData<FullEmployeeModel, dynamic>(sql,
                            new { Id = idNumber },
                            _connectionString).FirstOrDefault().IsActive;

        }

        protected override List<EmailAddressModel> GetEmployeeEmailAddressesById(int employeeId)
        {
            string sql = @"select e.*
                    from dbo.EmailAddresses e
                    inner join
                    dbo.EmployeeEmail ee on ee.EmailAddressId = e.Id
                    where ee.EmployeeId = @Id";

            return db.LoadData<EmailAddressModel, dynamic>(sql, new { Id = employeeId }, _connectionString);
        }

        protected override List<PhoneNumberModel> GetEmployeePhoneNumbersById(int employeeId)
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
