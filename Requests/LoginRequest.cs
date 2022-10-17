using Microsoft.Build.Framework;

namespace MVCIdentityBookRecords.Requests
{
    public class LoginRequest
    {
        public string? Email { get; set; } = null!;
        public String? Username { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
