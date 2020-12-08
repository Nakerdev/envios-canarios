namespace CanaryDeliveries.Backoffice.Api.Security
{
    public interface CryptoServiceProvider
    {
        Hash ComputeHash(string data);
    }

    public sealed class Hash
    {
        public string Value { get; }
        
        public Hash(string value)
        {
            Value = value;
        }
    }
}