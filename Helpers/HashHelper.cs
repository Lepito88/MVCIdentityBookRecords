using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace MVCIdentityBookRecords.Helpers
{
    public class HashHelper
    {
        public static byte[] GetSecureSalt()
        {
           
            return RandomNumberGenerator.GetBytes(32);
        }
        public static string HashUsingPbkdf2(string plaintext, byte[] salt)
        {
            byte[] derivedKey = KeyDerivation.Pbkdf2(plaintext, salt, KeyDerivationPrf.HMACSHA256, iterationCount: 100000, 32);

            return Convert.ToBase64String(derivedKey);
        }
    }
}
