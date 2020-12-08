using CanaryDeliveries.Backoffice.Api.Security;
using CanaryDeliveries.Backoffice.Api.Users.Login.Repositories;

namespace CanaryDeliveries.Backoffice.Api.Users.Login.Services
{
    public class LoginService
    {
        private readonly BackofficeUserRepository backofficeUserRepository;
        private readonly CryptoServiceProvider cryptoServiceProvider;

        public LoginService(
            BackofficeUserRepository backofficeUserRepository,
            CryptoServiceProvider cryptoServiceProvider)
        {
            this.backofficeUserRepository = backofficeUserRepository;
            this.cryptoServiceProvider = cryptoServiceProvider;
        }

        public virtual bool AreValidCredentials(LoginRequest request)
        {
            return backofficeUserRepository
                .SearchBy(request.Email)
                .Match(
                    None: () => false,
                    Some: user =>
                    {
                        var passwordIntentHash = cryptoServiceProvider.ComputeHash(request.Password);
                        return passwordIntentHash.Value == user.Password;
                    });
        }
        
        public sealed class LoginRequest
        {
            public string Email { get; }
            public string Password { get; }

            public LoginRequest(string email, string password)
            {
                Email = email;
                Password = password;
            }
        }
    }
}