namespace ERP.API.Models
{
    public class APIResponse<T>
    {
        public bool IsError { get; set; }
        public string Message { get; set; }
        public T data { get; set; }
        internal var Data { get; set; }
    }
}
