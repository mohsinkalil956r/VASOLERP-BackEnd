namespace ERP.DAL.DB.Entities
{
    public class AssetType : IBaseEntity
    {
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;
        public string Name { get; set; }
        public List<Asset> Assets { get; set; } = new();
    }
}