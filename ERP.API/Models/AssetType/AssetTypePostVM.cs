namespace ERP.API.Models.AssetType
{
    public class AssetTypePostVM
    {
      public  string Name { get; set; }

        public List<int> AssetsIds { get; set; } = new();
    }
}
