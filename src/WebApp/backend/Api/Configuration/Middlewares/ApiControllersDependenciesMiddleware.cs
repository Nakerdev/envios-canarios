using CanaryDeliveries.WebApp.Api.PurchaseApplication.Create.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace CanaryDeliveries.WebApp.Api.Configuration.Middlewares
{
    public static class ApiControllersDependenciesMiddleware
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(provider => Factory.CreatePurchaseApplicationCommandHandler());
        }
    }
}