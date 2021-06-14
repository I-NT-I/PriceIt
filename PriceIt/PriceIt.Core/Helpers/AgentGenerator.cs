using System.Security.Cryptography;

namespace PriceIt.Core.Helpers
{
    public class AgentGenerator
    {
        private const string Valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        public static string GetRandomCode(int length)
        {
            var code = "";
            using var provider = new RNGCryptoServiceProvider();
            while (code.Length != length)
            {
                var oneByte = new byte[1];
                provider.GetBytes(oneByte);
                var character = (char)oneByte[0];
                if (Valid.Contains(character))
                {
                    code += character;
                }
            }

            return code;
        }
    }
}
