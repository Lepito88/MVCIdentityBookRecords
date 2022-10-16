using MVCIdentityBookRecords.Models;
using MVCIdentityBookRecords.Responses.Books;

namespace MVCIdentityBookRecords.Responses.Authors
{
    public class AuthorResponse : BaseResponse
    {
        public int Idauthor { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public List<BooksResponseForAuthor> Books { get; set; }
    }
}
