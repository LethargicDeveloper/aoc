using System.Security.Cryptography;
using System.Text;

namespace AocLib;

public static class MD5Extensions
{
    extension(MD5)
    {
        public static string ComputeHash(string input)
        {

            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = MD5.HashData(bytes);

            var sb = new StringBuilder();
            foreach (var b in hash)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }
    }
}