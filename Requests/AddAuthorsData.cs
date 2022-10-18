using MVCIdentityBookRecords.Models;

namespace MVCIdentityBookRecords.Requests
{
    public class AddAuthorsData
    {
        public List<Author> Authors { get; set; }
        public List<Book> Books { get; set; }
        public List<Category> Categories { get; set; }
    }
}
