using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.DAL.DB.Entities
{
    public interface IBaseEntity
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }

    }
}
