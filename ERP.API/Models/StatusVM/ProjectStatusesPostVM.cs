namespace ERP.API.Models.StatusesVM
{
    public class ProjectStatusesPostVM
    {
        public bool IsActive { get; set; } = true;
        public string Name { get; set; }
        public bool IsProgress { get; set; }
        public int Progress { get; set; }
    }
}
