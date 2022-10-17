namespace MVCIdentityBookRecords.Responses.Users
{
    public class CreateUserResponse : BaseResponse
    {
        public string Iduser { get; set; }
        public string Username { get; set; } = null!;
        public string? Email { get; set; } = string.Empty;
    }
}
