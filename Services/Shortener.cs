using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace UrlShortener.Services {
    public class Shortener {
        public string GenerateShortKey(string originalUrl) {
            using (SHA256 sha256 = SHA256.Create()) {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(originalUrl));

                // Convert hash bytes to a positive numeric value (e.g., using BitConverter)
                long numericValue = BitConverter.ToInt64(hashBytes, 0);
                if (numericValue < 0) {
                    numericValue = -numericValue; // Ensure a positive value
                }

                // Encode the numeric value using Base62
                string shortKey = Base62Encode(numericValue);

                return $"http://localhost:44343/{shortKey}";
            }
        }

        private string Base62Encode(long value) {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder result = new StringBuilder();

            do {
                result.Insert(0, chars[(int)(value % 62)]);
                value /= 62;
            } while (value > 0);

            return result.ToString();
        }
    }
}
