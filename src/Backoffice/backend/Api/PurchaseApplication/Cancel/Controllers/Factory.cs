using CanaryDeliveries.Backoffice.Api.Configuration;
using CanaryDeliveries.PurchaseApplication.Domain.Cancel;
using CanaryDeliveries.PurchaseApplication.Domain.Services;
using CanaryDeliveries.PurchaseApplication.Repositories;

namespace CanaryDeliveries.Backoffice.Api.PurchaseApplication.Cancel.Controllers
{
    public static class Factory
    {
        public static CancelPurchaseApplicationCommandHandler CancelPurchaseApplicationCommandHandler ()
        {
            return new CancelPurchaseApplicationCommandHandler(
                purchaseApplicationRepository: new PurchaseApplicationEntityFrameworkRepository(
                    purchaseApplicationDbConnectionString: Environment.PurchaseApplicationDbConnectionString),
                timeService: new SystemTimeService());
        }
    }
}