namespace AppBackendCore2026.Models
{
    public class ResponseObj<T>
    {
        public bool success { get; set; }
        public ErrorType? error { get; set; }
        public string? message { get; set; }
        public T? data { get; set; }
    }
}