namespace ERP.API.Models.Contacts
{
    public class ContactsPostVM
    {
        public string Type { get; set; }
        public int? ReferenceId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }


    }
}
