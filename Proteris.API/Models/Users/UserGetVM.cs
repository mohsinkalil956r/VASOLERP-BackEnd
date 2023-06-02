namespace ERP.API.Models.Users
{
    public class UserGetVM
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public string Email { get; set; }
        public string Token { get; set; }

        public List<string> Roles { get; set; }
        public List<string> Permissions { get; set; }

    }
}
