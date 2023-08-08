using ERP.API.Models.EmployeeGetResponse;

namespace ERP.API.Models.EmployeeContactGetResponse
{
    public class EmployeeContactGetResponseVM
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }
        public EmployeeGetResponseVM  Employee { get; set; }
    }
}
