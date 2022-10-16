using MVCIdentityBookRecords.Models;
using MVCIdentityBookRecords.Requests;
using MVCIdentityBookRecords.Responses;
using MVCIdentityBookRecords.Responses.Categories;
using MVCIdentityBookRecords.Responses.Token;
using MVCIdentityBookRecords.Responses.Users;

namespace MVCIdentityBookRecords.Interfaces
{
    public interface IUserService
    {
        Task<GetUsersResponse> GetUsersAsync();
        Task<UserDetailResponse> GetUserByIdAsync(string id);
        Task<CreateUserResponse> CreateUserAsync(ApplicationUser user);
        Task<CreateUserResponse> UpdateUserAsync(string id, ApplicationUser user);
        Task<CreateUserResponse> DeleteUserAsync(string id);
    }
}
