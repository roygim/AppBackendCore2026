using System.ComponentModel.DataAnnotations;

namespace AppBackendCore2026.DTOs
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "firstname is required and must be at least 2 characters")]
        [MinLength(2, ErrorMessage = "firstname is required and must be at least 2 characters")]
        public string? firstname { get; set; }

        [Required(ErrorMessage = "lastname is required and must be at least 2 characters")]
        [MinLength(2, ErrorMessage = "lastname is required and must be at least 2 characters")]
        public string? lastname { get; set; }

        [Required(ErrorMessage = "email is required")]
        [EmailAddress(ErrorMessage = "invalid email")]
        public string email { get; set; } = string.Empty;

        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,12}$",
            ErrorMessage = "password must be 8-12 characters and include a letter, a number, and a special character (@$!%*#?&)")]
        public string password { get; set; } = string.Empty;
    }
}
