using CanaryDeliveries.WebApp.Api.PurchaseApplication.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace CanaryDeliveries.WebApp.Api.Configuration
{
    public static class ApiControllersDependenciesResolver
    {
        public static void Resolve(IServiceCollection services)
        {
            services.AddScoped(provider => Factory.CreatePurchaseApplicationCommandHandler());
        }
    }
}