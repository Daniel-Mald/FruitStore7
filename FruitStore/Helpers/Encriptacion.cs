using System.Security.Cryptography;
using System.Text;

namespace FruitStore.Helpers
{
    public static class Encriptacion
    {
        public static string StringToSHA512(string s)
        {
            using(var sha512 = SHA512.Create()) //using para que se destruya
            {
                var arreglo = Encoding.UTF8.GetBytes(s);
                var hash = sha512.ComputeHash(arreglo);
                return Convert.ToHexString(hash).ToLower();
            }
            
            

        }
        public static string FileToSHA512(string s)
        {
            using (var sha512 = SHA512.Create())
            {
                var arreglo = File.ReadAllBytes(s);
            var hash = sha512.ComputeHash(arreglo);
            return Convert.ToHexString(hash).ToLower();

            }
        }
    }
}
