using ERP.DAL.DB.Entities;

namespace ERP.API.Models.Assets
{
    public class AssetGetVM : AssetPostVM
    {
        public Employee Employees { get; set; }

    }
}
