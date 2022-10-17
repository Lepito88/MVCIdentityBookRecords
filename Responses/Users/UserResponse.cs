namespace MVCIdentityBookRecords.Responses.Users
{
    public class UserResponse
    {
        public string UserId { get; set; }
        public string Username { get; set; } = null!;
        public string? Email { get; set; } = string.Empty;
        public string? Firstname { get; set; } = string.Empty;
        public string? Lastname { get; set; } = string.Empty!;
        public string? PhoneNumber { get; set; } = string.Empty;
    }
}
