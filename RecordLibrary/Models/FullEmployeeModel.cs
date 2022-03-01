

namespace RecordLibrary.Models
{
    public class FullEmployeeModel
    {
        public BasicEmployeeModel? BasicInfo { get; set; }
        public List<EmailAddressModel> EmailAddresses { get; set; } = new();
        public List<PhoneNumberModel> PhoneNumbers { get; set; } = new();
        public bool IsActive { get; set; }
        public Decimal Salary { get; set; }
        public DateTime DateHired { get; set; }
    }
}
