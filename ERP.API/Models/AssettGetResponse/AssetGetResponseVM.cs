using ERP.API.Models.AssetTypeGetResponse;
using ERP.API.Models.EmployeeGetResponse;
using ERP.DAL.DB.Entities;

namespace ERP.API.Models.AssettGetResponse
{
    public class AssetGetResponseVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime PurchaseDate { get; set; }
        public double PurchasePrice { get; set; }
        public AssetTypeGetResponseVM AssetType { get; set; }
        public EmployeeGetResponseVM Employees { get; set; }

    }
}
