namespace MVCIdentityBookRecords.Requests
{
    public class RefreshTokenRequest
    {
        public int Iduser { get; set; }
        public string RefreshToken { get; set; }
    }
}
