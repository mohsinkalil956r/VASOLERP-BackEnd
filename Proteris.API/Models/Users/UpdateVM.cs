using System.ComponentModel.DataAnnotations;

namespace ERP.API.Models.Users
{
    public class UpdateVM
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public List<string> Permissions { get; set; }

    }
}
