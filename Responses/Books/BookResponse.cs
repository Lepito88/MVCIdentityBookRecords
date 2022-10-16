using MVCIdentityBookRecords.Models;

namespace MVCIdentityBookRecords.Responses.Books
{
    public class BookResponse : BaseResponse
    {
        public int Idbook { get; set; }
        public string BookName { get; set; }
        public string Type { get; set; }
        public string Isbn { get; set; }
        public DateTime ReleaseYear { get; set; }

        public ICollection<Category> Categories { get; set; }
        public ICollection<Author> Authors { get; set; }
    }
}
