using System.ComponentModel.DataAnnotations;

namespace AppBackendCore2026.Models
{
    public class UserObj
    {
        public int Id { get; set; }
        public string? firstname { get; set; }
        public string? lastname { get; set; }
        public string email { get; set; } = string.Empty;
        public string? Password { get; set; }
    }
}