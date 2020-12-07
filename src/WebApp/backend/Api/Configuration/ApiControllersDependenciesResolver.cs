using CanaryDeliveries.PurchaseApplication.Domain.Create;
using CanaryDeliveries.PurchaseApplication.Domain.Services;
using CanaryDeliveries.PurchaseApplication.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CanaryDeliveries.WebApp.Api.Configuration
{
    public static class ApiControllersDependenciesResolver
    {
        public static void Resolve(IServiceCollection services)
        {
            services.AddScoped(provider => new CreatePurchaseApplicationCommandHandler(
                purchaseApplicationRepository: new PurchaseApplicationEntityFrameworkRepository(),
                timeService: new SystemTimeService()));
        }
    }
}