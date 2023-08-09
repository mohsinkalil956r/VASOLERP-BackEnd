namespace ERP.API.Models.StatusGetResponse
{
    public class StatusGetResponseVM
    {
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;
        public string Name { get; set; }
        public bool IsProgress { get; set; }
        public int Progress { get; set; }
    }
}
