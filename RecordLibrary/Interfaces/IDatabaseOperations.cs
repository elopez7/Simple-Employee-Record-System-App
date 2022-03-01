using RecordLibrary.Models;


namespace RecordLibrary.Interfaces
{
    public interface IDatabaseOperations
    {
        protected void SaveEmployeeBasicInformation(FullEmployeeModel employee);
        protected int GetEmployeeIdByNameAndLastName(FullEmployeeModel employee);
        protected void SaveEmployeePhoneNumbers(FullEmployeeModel employee, int employeeId);
        protected void SaveEmployeeEmailAddresses(FullEmployeeModel employee, int employeeId);
        protected List<BasicEmployeeModel> GetAllEmployeesBasicInformation();
        protected BasicEmployeeModel GetBasicEmployeeById(int idNumber);
        protected void UpdateEmployeeStatusById(int idNumber);
        protected void UpdateEmployeeSalaryById(int idNumber, bool isPromotion = true);
        protected decimal CalculateNewSalary(decimal currentSalary, bool isPromotion);
        protected decimal GetEmployeeSalaryById(int idNumber);
        protected bool GetEmployeeCurrentStatus(int idNumber);
        protected List<EmailAddressModel> GetEmployeeEmailAddressesById(int employeeId);
        protected List<PhoneNumberModel> GetEmployeePhoneNumbersById(int employeeId);

    }
}
