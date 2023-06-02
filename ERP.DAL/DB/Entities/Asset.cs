using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.DAL.DB.Entities
{
    public class Asset : IBaseEntity
    {
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime PurchaseDate { get; set; }
        public double PurchasePrice { get; set; }
        public int AssetTypeId { get; set; }
        public AssetType AssetType { get; set; }
        public List<AssetIssuance> AssetIssuances { get; set; } = new();

        public List<Employee> Employees { get; set; } = new();

    }
}
