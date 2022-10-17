using MVCIdentityBookRecords.Models;

namespace MVCIdentityBookRecords.Responses.Books
{
    public class BooksResponseForAuthor : BaseResponse
    {
        public int BookId { get; set; }

        public string BookName { get; set; }
        public string Type { get; set; }
        public string Isbn { get; set; }
        public DateTime ReleaseYear { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}
