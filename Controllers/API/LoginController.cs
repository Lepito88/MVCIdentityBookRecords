//using MVCIdentityBookRecords.Data;
//using MVCIdentityBookRecords.Models;
//using MVCIdentityBookRecords.Interfaces;
//using MVCIdentityBookRecords.Requests;
//using MVCIdentityBookRecords.Responses.Token;
//using MVCIdentityBookRecords.Services;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace MVCIdentityBookRecords.Controllers.API
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class LoginController : BaseApiController
//    {
//        private readonly ILoginService _loginService;
//        private readonly ITokenService _tokenService;

//        public LoginController(ILoginService loginService, ITokenService tokenService)
//        {
//            _loginService = loginService;
//            _tokenService = tokenService;
//        }
    
//        [HttpPost]
//        public async Task<IActionResult> Login(LoginRequest loginRequest)
//        {
//            if (loginRequest == null || (string.IsNullOrEmpty(loginRequest.Username) && string.IsNullOrEmpty(loginRequest.Email))|| string.IsNullOrEmpty(loginRequest.Password))
//            {
//                return BadRequest(new TokenResponse
//                {
//                    Error = "Missing login details",
//                    ErrorCode = "L01"
//                });
//            }

//            var loginResponse = await _loginService.LoginAsync(loginRequest);

//            if (!loginResponse.Success)
//            {
//                return Unauthorized(new
//                {
//                    loginResponse.ErrorCode,
//                    loginResponse.Error
//                });
//            }

//            return Ok(loginResponse);
//        }

//        [Authorize]
//        [HttpPost]
//        [Route("refresh_token")]
//        public async Task<IActionResult> RefreshToken(RefreshTokenRequest refreshTokenRequest)
//        {
//            if (refreshTokenRequest == null || string.IsNullOrEmpty(refreshTokenRequest.RefreshToken) || refreshTokenRequest.Iduser == 0)
//            {
//                return BadRequest(new TokenResponse
//                {
//                    Error = "Missing refresh token details",
//                    ErrorCode = "R01"
//                });
//            }

//            var validateRefreshTokenResponse = await _tokenService.ValidateRefreshTokenAsync(refreshTokenRequest);

//            if (!validateRefreshTokenResponse.Success)
//            {
//                return UnprocessableEntity(validateRefreshTokenResponse);
//            }
//            var tmpuser = new ApplicationUser
//            {
//                Id = validateRefreshTokenResponse.Iduser,
//            };
//            var tokenResponse = await _tokenService.GenerateTokensAsync(tmpuser);
//            //var tokenResponse = await _tokenService.GenerateTokensAsync(validateRefreshTokenResponse.Iduser);

//            return Ok(new { AccessToken = tokenResponse.Item1, Refreshtoken = tokenResponse.Item2 });
//        }

//        [HttpPost]
//        [Route("register")]
//        public async Task<IActionResult> Signup(RegisterRequest registerRequest)
//        {
//            if (!ModelState.IsValid)
//            {
//                var errors = ModelState.Values.SelectMany(x => x.Errors.Select(c => c.ErrorMessage)).ToList();
//                if (errors.Any())
//                {
//                    return BadRequest(new TokenResponse
//                    {
//                        Error = $"{string.Join(",", errors)}",
//                        ErrorCode = "S01"
//                    });
//                }
//            }

//            var registerResponse = await _loginService.RegisterAsync(registerRequest);

//            if (!registerResponse.Success)
//            {
//                return UnprocessableEntity(registerResponse);
//            }

//            return Ok(registerResponse);
//        }

//        [Authorize]
//        [HttpPost]
//        [Route("logout")]
//        public async Task<IActionResult> Logout(int Iduser)
//        {
//            var logout = await _loginService.LogoutAsync(Iduser);

//            if (!logout.Success)
//            {
//                return UnprocessableEntity(logout);
//            }

//            return Ok("Logged out");
//        }
//    }
//}
