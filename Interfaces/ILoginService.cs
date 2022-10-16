using MVCIdentityBookRecords.Requests;
using MVCIdentityBookRecords.Responses.Token;
using MVCIdentityBookRecords.Responses;

namespace MVCIdentityBookRecords.Interfaces
{
    public interface ILoginService
    {
        Task<TokenResponse> LoginAsync(LoginRequest loginRequest);
        Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest);
        Task<LogoutResponse> LogoutAsync(int Iduser);
    }
}
