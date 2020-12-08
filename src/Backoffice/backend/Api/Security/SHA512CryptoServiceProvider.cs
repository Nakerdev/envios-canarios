using System.Security.Cryptography;
using System.Text;

namespace CanaryDeliveries.Backoffice.Api.Security
{
    public sealed class SHA512CryptoServiceProvider : CryptoServiceProvider
    {
        private readonly SHA512 sha512;

        public SHA512CryptoServiceProvider(SHA512 sha512)
        {
            this.sha512 = sha512;
        }

        public Hash ComputeHash(string data)
        {
            var dataBytes = Encoding.ASCII.GetBytes(data);
            var result = sha512.ComputeHash(dataBytes);
            return new Hash(value: result.ToString());
        }
    }
}