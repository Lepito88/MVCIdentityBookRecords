namespace MVCIdentityBookRecords.Responses
{
    public class AuthorToBookResponse : BaseResponse
    {
        public int AuthorId { get; set; }
        public int BookId { get; set; }
        public string BookName { get; set; }
    }
}
