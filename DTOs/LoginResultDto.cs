namespace AppBackendCore2026.DTOs
{
    public class LoginResultDto
    {
        public string accessToken { get; set; } = string.Empty;
        public UserLightDto user { get; set; } = new();
    }
}
