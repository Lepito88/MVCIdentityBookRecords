//using MVCIdentityBookRecords.Models;
//using Microsoft.CodeAnalysis.CSharp.Syntax;
//using Microsoft.IdentityModel.Tokens;
//using System.Configuration;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Security.Cryptography;

//namespace MVCIdentityBookRecords.Helpers
//{
//    public class TokenHelper
//    {
        
//        private readonly IConfiguration _configuration;

//        public TokenHelper(IConfiguration config) {
//            _configuration = config;
//        }

//        public string JwtIssuer { get; private set; }
//        public string JwtAudience { get; private set; }
//        public string JwtSecret { get; private set; }
        
//        public async Task<string> GenerateAccessToken(ApplicationUser user)
//        {

//            JwtIssuer = _configuration["AppSettings:JWT_Issuer"];
//            JwtAudience = _configuration["AppSettings:JWT_Audience"];
//            JwtSecret = _configuration["AppSettings:JWT_Secret"];
          
//            var tokenHandler = new JwtSecurityTokenHandler();
//            var key = Convert.FromBase64String(JwtSecret);

//            var claimsIdentity = new ClaimsIdentity(new[] {
//                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
//                new Claim(ClaimTypes.Email, user.Email),
//            });

//            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

//            var tokenDescriptor = new SecurityTokenDescriptor
//            {
//                Subject = claimsIdentity,
//                Issuer = JwtIssuer,
//                Audience = JwtAudience,
//                Expires = DateTime.Now.AddMinutes(15),
//                SigningCredentials = signingCredentials,

//            };
//            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

//            return await System.Threading.Tasks.Task.Run(() => tokenHandler.WriteToken(securityToken));
//        }
//        public static async Task<string> GenerateRefreshToken()
//        {
//            var secureRandomBytes = new byte[32];

//            using var randomNumberGenerator = RandomNumberGenerator.Create();
//            await System.Threading.Tasks.Task.Run(() => randomNumberGenerator.GetBytes(secureRandomBytes));

//            var refreshToken = Convert.ToBase64String(secureRandomBytes);
//            return refreshToken;
//        }
//    }
//}