using ERP.DAL.DB.ClientContactDTO;
using ERP.DAL.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.DAL.Repositories.Abstraction
{
    public interface IClientsRepository : IRepository<Client>
    {
        IQueryable<ClientContactDTO> GetClientWithContact();
    }
}
