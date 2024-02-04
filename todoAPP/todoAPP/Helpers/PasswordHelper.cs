using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace todoAPP;

public static class PasswordHelper
{
  public static byte[] CreateSalt()
  {
    byte[] randomBytes = new byte[128 / 8];
    RandomNumberGenerator generator = RandomNumberGenerator.Create();
    generator.GetBytes(randomBytes);
    return randomBytes;
  }

  public static string GeneratePassword(string password, byte[] salt)
  {
    var key = KeyDerivation.Pbkdf2(
        password: password,
        salt: salt,//鹽
        prf: KeyDerivationPrf.HMACSHA512,//偽隨機函數
        iterationCount: 1000,//雜湊執行次數
        numBytesRequested: 256 / 8
        );
    return Convert.ToBase64String(key);
  }

  public static bool ValidatePassword(string targetPassword, string targetSalt, string inputPassword)
  {
    var derivedPassword = GeneratePassword(
        inputPassword,
        Convert.FromBase64String(targetSalt)
    );

    return KeyDerivation.Equals(targetPassword, derivedPassword);
  }
}
