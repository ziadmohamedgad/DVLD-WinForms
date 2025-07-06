using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
namespace DVLD_Hash
{
    public class clsHash
    {
        public static string GenerateRandomSalt(int size = 16) //if you changed the default value (16), you have to be concern about the column capacity of chars at database file.
        {
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] saltBytes = new byte[size];
                rng.GetBytes(saltBytes); //fill saltBytes
                return Convert.ToBase64String(saltBytes).ToLower();
            }
        }
        public static string ComputeHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}