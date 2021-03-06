using Microsoft.Extensions.DependencyInjection;

namespace CanaryDeliveries.Backoffice.Api.Configuration.Middleware
{
    public static class ApiControllersDependenciesMiddleware
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(provider => Users.Login.Controllers.Factory.LoginService());
            services.AddScoped(provider => PurchaseApplication.Search.Controllers.Factory.PurchaseApplicationRepository());
            services.AddScoped(provider => PurchaseApplication.Cancel.Controllers.Factory.CancelPurchaseApplicationCommandHandler());
        }
    }
}