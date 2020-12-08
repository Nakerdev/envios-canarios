using CanaryDeliveries.Backoffice.Api.Auth;
using CanaryDeliveries.Backoffice.Api.Configuration;
using CanaryDeliveries.Backoffice.Api.Users.Login.Repositories;
using CanaryDeliveries.Backoffice.Api.Users.Login.Services;
using SHA512CryptoServiceProvider = CanaryDeliveries.Backoffice.Api.Security.SHA512CryptoServiceProvider;

namespace CanaryDeliveries.Backoffice.Api.Users.Login.Controllers
{
    public static class Factory
    {
        public static LoginController LoginController()
        {
            return new LoginController(loginService: LoginService());

            LoginService LoginService()
            {
                return new LoginService(
                    backofficeUserRepository: new BackofficeUserStaticRepository(),
                    cryptoServiceProvider: new SHA512CryptoServiceProvider(),
                    tokenHandler: new JsonWebTokenHandler(Environment.JsonWebTokenSecretKey));
            }
        }
    }
}