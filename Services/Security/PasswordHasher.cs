using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.Security
{
    public class PasswordHasher
    {
        public static byte[] HashPassword(string password, out byte[] salt)
        {
            salt = GenerateSalt();
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashedBytes = ComputeHash(passwordBytes, salt);
            return hashedBytes;
        }

        public static bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] computedHash = ComputeHash(passwordBytes, storedSalt);
            return storedHash.Length == computedHash.Length && storedHash.AsSpan().SequenceEqual(computedHash);
        }

        private static byte[] GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes); 
            }
            return saltBytes;
        }

        private static byte[] ComputeHash(byte[] passwordBytes, byte[] saltBytes)
        {
            byte[] combinedBytes = new byte[passwordBytes.Length + saltBytes.Length];
            Buffer.BlockCopy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
            Buffer.BlockCopy(saltBytes, 0, combinedBytes, passwordBytes.Length, saltBytes.Length);
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(combinedBytes);
            }
        }
    }
}
