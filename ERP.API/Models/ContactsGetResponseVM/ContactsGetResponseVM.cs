﻿namespace ERP.API.Models.ContactsGetResponseVM
{
    public class ContactsGetResponseVM
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string DepartmentName { get; set; }
        public string Type { get; set; }
    }
}
