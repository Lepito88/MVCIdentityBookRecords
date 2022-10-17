namespace MVCIdentityBookRecords.Requests
{
    public class RelationshipRequest
    {
        public string UserId { get; set; } = string.Empty!;
        public int BookId { get; set; } = 0!;
        public int AuthorId { get; set; } = 0!;
        public int CategoryId { get; set; } = 0!; 
    }
}
