using System.ComponentModel.DataAnnotations;

namespace ERP.API.Models.AssetType
{
    public class AssetTypePostVM
    {
        [Required]
        public  string Name { get; set; }

    }
}
