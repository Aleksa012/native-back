using System.Security.Cryptography;
using System.Text;
namespace NativeBackendApp.Util;

public class PasswordHasher
{
    public static void HashUserPassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }

    public static bool VerifyHash(string password, byte[] hash, byte[] salt)
    {
        using (var hmac = new HMACSHA512(salt))
        {
            return hash.SequenceEqual(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
    }
}