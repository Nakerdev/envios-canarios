using CanaryDeliveries.Backoffice.Api.Users.Login.Repositories;

namespace CanaryDeliveries.Backoffice.Api.Auth
{
    public interface TokenHandler
    {
        Token Create(BackofficeUser user);
    }

    public sealed class Token
    {
        public string Value { get; }
        
        public Token(string value)
        {
            Value = value;
        }
    }
}