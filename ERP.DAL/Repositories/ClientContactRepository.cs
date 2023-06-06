using ERP.DAL.DB.Entities;
using ERP.DAL.DB;
using ERP.DAL.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.DAL.Repositories
{
    public class ClientContactRepository : BaseRepository<ClientContact>, IClientContactRepository
    {
        public ClientContactRepository(ERPContext context) : base(context)
        {

        }
    }
}