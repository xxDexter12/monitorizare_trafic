using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace monitorizare_trafic.Utils
{
    public class SecurityHelper
    {
        /// <summary>
        /// Compute the SHA-256 hash for a given input string.
        /// </summary>
        /// <param name="input">The string to hash.</param>
        /// <returns>The computed hash as a hexadecimal string.</returns>
        public static string ComputeHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder hashStringBuilder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hashStringBuilder.Append(b.ToString("x2"));
                }
                return hashStringBuilder.ToString();
            }
        }
    }
}
