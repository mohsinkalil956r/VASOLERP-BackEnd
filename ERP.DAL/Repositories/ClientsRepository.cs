using ERP.DAL.DB.Entities;
using ERP.DAL.DB;
using ERP.DAL.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.DAL.DB.ClientContactDTO;

namespace ERP.DAL.Repositories
{
    public class ClientsRepository : BaseRepository<Client>, IClientsRepository
    {
        private readonly ERPContext _context;
        public ClientsRepository(ERPContext context) : base(context)
        {
            this._context = context;
        }

        public IQueryable<ClientContactDTO> GetClientWithContact()
        {
            var query = from c in this._context.Contacts
                        where c.ReferenceId.HasValue && c.Type == "Client"
                        join e in this._context.Clients on c.ReferenceId.Value equals e.Id
                        select new ClientContactDTO
                        {
                            Client = e,
                            Contact = c
                        };

            return query;

        }

    }
}
