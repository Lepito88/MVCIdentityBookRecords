using MVCIdentityBookRecords.Models;
using MVCIdentityBookRecords.Requests;
using MVCIdentityBookRecords.Responses;

namespace MVCIdentityBookRecords.Interfaces
{
    public interface IEntityRelationShipManagerService
    {
        public Task<BookToUserResponse> AddBookToUserAsync(RelationshipRequest request);
        public Task<BookToUserResponse> RemoveBookFromUserAsync(RelationshipRequest request);

        public Task<CategoryToBookResponse> AddCategoryToBookAsync(RelationshipRequest request);
        public Task<CategoryToBookResponse> RemoveCategoryFromBookAsync(RelationshipRequest request);

        public Task<AuthorToBookResponse> AddAuthorToBookAsync(RelationshipRequest request);
        public Task<AuthorToBookResponse> RemoveAuthorFromBookAsync(RelationshipRequest request);
    }
}
