using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.DAL.DB.Entities
{
    public class ClientContact : IBaseEntity
    {
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }

        //public int ClientId { get; set; }
        //public Client Client { get; set; }

    }
}