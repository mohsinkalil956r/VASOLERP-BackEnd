using ERP.DAL.DB.Entities;

namespace ERP.API.Models.Assets
{
    public class AssetGetVM:AssetPostVM
    {
        public AssetType AssetType { get; set; }
        public Employee Employees { get; set; }

    }
}
