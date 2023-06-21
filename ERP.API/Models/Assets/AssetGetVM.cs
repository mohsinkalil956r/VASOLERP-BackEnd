using ERP.DAL.DB.Entities;

namespace ERP.API.Models.Assets
{
    public class AssetGetVM
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime PurchaseDate { get; set; }
        public double PurchasePrice { get; set; }
        public int AssetTypeId { get; set; }
        public Employee Employees { get; set; }

    }
}
