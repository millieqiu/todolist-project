using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace todoAPP.Services
{
    public class AuthService
    {
        public byte[] CreateSalt()
        {
            byte[] randomBytes = new byte[128 / 8];
            RandomNumberGenerator generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomBytes);
            return randomBytes;
        }

        public string PasswordGenerator(string password, byte[] salt)
        {
            byte[] key = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,//鹽
                prf: KeyDerivationPrf.HMACSHA512,//偽隨機函數
                iterationCount: 1000,//雜湊執行次數
                numBytesRequested: 256 / 8
                );
            return Convert.ToBase64String(key);
        }

        public bool PasswordValidator(string targetPassword, string targetSalt, string inputPassword)
        {
            string derivedPassword = PasswordGenerator(
                inputPassword,
                Convert.FromBase64String(targetSalt)
            );

            return KeyDerivation.Equals(targetPassword, derivedPassword);
        }
    }
}

