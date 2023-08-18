using ERP.DAL.DB;
using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;



namespace ERP.DAL.Repositories
{
    public class ContactRepository : BaseRepository<Contact>, IContactRepository
    {
        public ContactRepository(ERPContext context)
            : base(context)
        {

        }
    }
}
