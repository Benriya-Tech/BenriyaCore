using System;
using System.Linq;
using System.Security.Cryptography;

namespace Benriya.Utils
{
    public class RNG
    {
        public static string RandomString()
        {
            Guid g = Guid.NewGuid();
            string str = Convert.ToBase64String(g.ToByteArray());
            str = str.Replace("=", string.Empty);
            str = str.Replace("+", string.Empty);
            str = str.Replace("/", string.Empty);
            return str;
        }

        public static string UniqueString() => (((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds() + Guid.NewGuid().ToString("N"));


        public static string GenerateString(int length)
        {
            string characters = "abcdefghijklmnopqrstuvwxyz0123456789";
            var bytes = new byte[length];
            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(bytes);
            }
            return new string(bytes.Select(x => characters[x % characters.Length]).ToArray());
        }

    }
}
