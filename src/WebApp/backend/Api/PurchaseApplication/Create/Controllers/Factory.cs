using CanaryDeliveries.PurchaseApplication.Domain.Create;
using CanaryDeliveries.PurchaseApplication.Domain.Services;
using CanaryDeliveries.PurchaseApplication.Repositories;
using CanaryDeliveries.WebApp.Api.Configuration;

namespace CanaryDeliveries.WebApp.Api.PurchaseApplication.Create.Controllers
{
    public static class Factory
    {
        public static CreatePurchaseApplicationCommandHandler CreatePurchaseApplicationCommandHandler()
        {
            return new CreatePurchaseApplicationCommandHandler(
                purchaseApplicationRepository: PurchaseApplicationRepository(),
                timeService: new SystemTimeService());

            PurchaseApplicationEntityFrameworkRepository PurchaseApplicationRepository()
            {
                return new PurchaseApplicationEntityFrameworkRepository(
                    purchaseApplicationDbConnectionString: Environment.PurchaseApplicationDbConnectionString);
            }
        }
    }
}