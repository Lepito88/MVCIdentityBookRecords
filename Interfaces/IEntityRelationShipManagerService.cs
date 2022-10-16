using MVCIdentityBookRecords.Models;
using MVCIdentityBookRecords.Responses;

namespace MVCIdentityBookRecords.Interfaces
{
    public interface IEntityRelationShipManagerService
    {
        public Task<BookToUserResponse> AddBookToUserAsync(string userid, int bookid);
        public Task<BookToUserResponse> RemoveBookFromUserAsync(string userid, int bookid);

        public Task<CategoryToBookResponse> AddCategoryToBookAsync(int bookid, int categoryid);
        public Task<CategoryToBookResponse> RemoveCategoryFromBookAsync(int bookid, int categoryid);

        public Task<AuthorToBookResponse> AddAuthorToBookAsync(int bookid, int authorid);
        public Task<AuthorToBookResponse> RemoveAuthorFromBookAsync(int bookid, int authorid);
    }
}
