using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.DAL.DB.Entities
{
    public class AssetIssuance  : IBaseEntity
    {
        
        public bool IsActive { get; set; } = true;
        public int EmployeeId { get; set; }
        public int AssetId { get; set; }
        public DateTime IssuanceDate { get; set; }

        public Employee Employee { get; set; }
        public Asset Asset { get; set; }


        [NotMapped]
        public int Id { get; set; }
    }
}
