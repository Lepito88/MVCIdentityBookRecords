using MVCIdentityBookRecords.Models;
using MVCIdentityBookRecords.Requests;
using MVCIdentityBookRecords.Responses.Token;

namespace MVCIdentityBookRecords.Interfaces
{
    public interface ITokenService
    {
        Task<Tuple<string, string>> GenerateTokensAsync(ApplicationUser user);
        Task<ValidateRefreshTokenResponse> ValidateRefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
        Task<bool> RemoveRefreshTokenAsync(ApplicationUser user);
    }
}
