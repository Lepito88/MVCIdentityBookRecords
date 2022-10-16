namespace MVCIdentityBookRecords.Responses.Users
{
    public class UserResponse
    {
        public int Iduser { get; set; }
        public string Username { get; set; } = null!;
        public string? Email { get; set; } = string.Empty;
        public string? Firstname { get; set; } = string.Empty;
        public string? Lastname { get; set; } = string.Empty!;
    }
}
