using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Security.Cryptography;
using System.Text;

namespace JWT_API_BD.Resources
{
    public class Utilities
    {
        public static string encryptPassword(string password)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding encoding = Encoding.UTF8;

                byte[] result = hash.ComputeHash(encoding.GetBytes(password)); 

                foreach (byte b in result)
                {
                    sb.Append(b.ToString("x2"));
                }
            }

            return sb.ToString();
        } 
    }
}
