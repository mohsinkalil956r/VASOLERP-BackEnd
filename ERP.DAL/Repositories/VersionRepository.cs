using ERP.DAL.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.DAL.Repositories
{
    public class VersionRepository : IVersionRepository
    {
        public string GetVersion()
        {
            // You can put your version logic here
            string version = "1.0.0"; // Hardcoded version

            return version;
        }
    }
}
