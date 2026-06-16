namespace AppBackendCore2026.DTOs
{
    public class UserLightDto
    {
        public int id { get; set; }
        public string? firstname { get; set; }
        public string? lastname { get; set; }
        public string email { get; set; } = string.Empty;
    }
}
