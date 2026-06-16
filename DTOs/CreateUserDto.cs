namespace AppBackendCore2026.DTOs
{
    public class CreateUserDto
    {
        public string? firstname { get; set; }
        public string? lastname { get; set; }
        public string email { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
    }
}
