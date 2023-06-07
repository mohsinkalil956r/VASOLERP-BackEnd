using ERP.DAL.DB.Entities;

namespace ERP.API.Models.Assets
{
    public class AssetPostVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime PurchaseDate { get; set; }
        public double PurchasePrice { get; set; }
        public int AssetTypeId { get; set; }
      
    }
}
