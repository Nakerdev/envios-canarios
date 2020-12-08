using CanaryDeliveries.Backoffice.Api.Auth;
using CanaryDeliveries.Backoffice.Api.Configuration;
using CanaryDeliveries.Backoffice.Api.Security;
using CanaryDeliveries.Backoffice.Api.Users.Login.Repositories;
using CanaryDeliveries.Backoffice.Api.Users.Login.Services;

namespace CanaryDeliveries.Backoffice.Api.Users.Login.Controllers
{
    public static class Factory
    {
        public static LoginService LoginService()
        {
            return new LoginService(
                backofficeUserRepository: new BackofficeUserStaticRepository(),
                cryptoServiceProvider: new BCryptCryptoServiceProvider(),
                tokenHandler: new JsonWebTokenHandler(Environment.JsonWebTokenSecretKey));
        }
    }
}