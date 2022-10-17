using MVCIdentityBookRecords.Models;
using MVCIdentityBookRecords.Responses.Authors;

namespace MVCIdentityBookRecords.Interfaces
{
    public interface IAuthorService
    {
        Task<GetAuthorsResponse> GetAuthorsAsync();
        Task<AuthorResponse> GetAuthorByIdAsync(int id);
        Task<AuthorResponse> CreateAuthorAsync(Author author);
        Task<AuthorResponse> UpdateAuthorAsync(int id, Author author);
        Task<AuthorResponse> DeleteAuthorAsync(int id);
    }
}
