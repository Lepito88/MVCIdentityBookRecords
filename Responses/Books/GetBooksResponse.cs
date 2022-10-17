using MVCIdentityBookRecords.Models;

namespace MVCIdentityBookRecords.Responses.Books
{
    public class GetBooksResponse : BaseResponse
    {
        public List<Book> Books { get; set; }
    }
}
