using MVCIdentityBookRecords.Models;
using MVCIdentityBookRecords.Responses.Books;

namespace MVCIdentityBookRecords.Interfaces
{
    public interface IBookService
    {
        Task<GetBooksResponse> GetBooksAsync();
        Task<BookResponse> GetBookByIdAsync(int id);
        Task<BookResponse> UpdateBookAsync(int id, Book book);
        Task<BookResponse> CreateBookAsync(Book book);
        Task<BookResponse> DeleteBookAsync(int id);
    }
}
