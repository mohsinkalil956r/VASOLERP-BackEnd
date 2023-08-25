using ERP.DAL.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.DAL.DB.ClientContactDTO
{
    public class ClientContactDTO
    {
        public Client Client { get; set; }
        public Contact Contact { get; set; }
    }
}
