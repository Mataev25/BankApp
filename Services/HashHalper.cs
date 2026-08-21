using System;
using System.Security.Cryptography;
using System.Text;

namespace BankApp.Services {
    public static class HashHelper {
        public static string HashPassword(string input) {
            if (string.IsNullOrEmpty(input))
                return "";

            SHA256 sha256 = null;
            try {
                sha256 = SHA256.Create();
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                string hash = BitConverter.ToString(bytes);
                hash = hash.Replace("-", "");
                hash = hash.ToLower();
                return hash;
            } finally {
                if (sha256 != null)
                    sha256.Dispose();
            }
        }
    }
}