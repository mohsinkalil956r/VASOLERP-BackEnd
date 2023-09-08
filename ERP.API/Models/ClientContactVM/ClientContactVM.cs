namespace ERP.API.Models.ClientContactVM
{
    public class ClientContactVM
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ContactViewModel contact { get; set; }
    }
}
