using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Benriya.Utils
{

    public class CryptographyCore
    {        
        // https://www.devtrends.co.uk/blog/hashing-encryption-and-random-in-asp.net-core
        public static string PBKDF2_hash(string password)
        {
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, 32)
            {
                IterationCount = 10000
            };
            byte[] hash = rfc2898DeriveBytes.GetBytes(20);
            byte[] salt = rfc2898DeriveBytes.Salt;
            return Convert.ToBase64String(salt) + "|" + Convert.ToBase64String(hash);
        }

        public static string SHA512_hash(string input)
        {
            using (var algorithm = SHA512.Create())
            {
                var hashedBytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public static string SHA256_hash(string input)
        {
            using (var algorithm = SHA256.Create())
            {
                var hashedBytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public static string MD5_hash(string input)
        {
            using (var algorithm = MD5.Create())
            {
                var hashedBytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
        public static string CreateCheckSUm_File(string filename)
        {
                using (var stream = File.OpenRead(filename))
                {
                    return CreateCheckSUm_Stream(stream);
                }
        }
        public static string CreateCheckSUm_Stream(Stream stream)
        {

            using (var algorithm = SHA512.Create())
            {
                var hashedBytes = algorithm.ComputeHash(stream);
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();                
            }
        }
    }


    public class PasswordUtils
    {
        private static string key = "SUnnd_rB3nriy@C0R3";
        public static void CreatePasswordHash(string username, string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (username == null || password == null) throw new ArgumentNullException("Username or password");
            var email = new EmailAddressAttribute();
            if (email.IsValid(username))
                username = $"{username.Substring(0, username.IndexOf('@'))}{key}";
            else
                username = username + key;
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password + username));
            }
        }

        public static bool VerifyPasswordHash(string username, string password, byte[] storedHash, byte[] storedSalt)
        {
            if (username == null || password == null) throw new ArgumentNullException("Username or password is required");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");
            var email = new EmailAddressAttribute();
            if (email.IsValid(username))
                username = $"{username.Substring(0, username.IndexOf('@'))}{key}";
            else
                username = username + key;
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password + username));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }
            return true;
        }
    }

}
