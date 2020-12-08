using CanaryDeliveries.Backoffice.Api.Users.Login.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace CanaryDeliveries.Backoffice.Api.Configuration.Middleware
{
    public static class ApiControllersDependenciesMiddleware
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(provider => Factory.LoginController());
        }
    }
}