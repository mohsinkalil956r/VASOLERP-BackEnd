namespace ERP.API.Models.Contacts
{
    public class ContactsPostVM
    {
        public string Type { get; set; }
        public int? ReferenceId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }


    }
}
