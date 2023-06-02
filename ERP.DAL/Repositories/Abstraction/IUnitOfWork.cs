using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.DAL.Repositories.Abstraction
{
    public interface IUnitOfWork
    {
        public UserRepository UserRepository { get; }
        public Task CommitAsync();
        
    }
}
