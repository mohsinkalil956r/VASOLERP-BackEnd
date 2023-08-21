using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.DAL.DB.Entities
{
    public class Contact : IBaseEntity
    {
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;

        public string Type { get; set; }
        public int? ReferenceId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }


    }
}
