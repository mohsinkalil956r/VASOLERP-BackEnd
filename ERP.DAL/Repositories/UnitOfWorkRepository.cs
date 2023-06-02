using ERP.DAL.DB;
using ERP.DAL.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.DAL.Repositories
{
    public class UnitOfWorkRepository : IUnitOfWork
    {
        private readonly ERPContext _context;

        public UnitOfWorkRepository(ERPContext context)
        {
            this._context = context;
        }

        private UserRepository _userRepository;

        public UserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    this._userRepository = new UserRepository(this._context);
                }
                return _userRepository;
            }
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
