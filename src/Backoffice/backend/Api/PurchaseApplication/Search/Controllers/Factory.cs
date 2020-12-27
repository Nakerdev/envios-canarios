using CanaryDeliveries.Backoffice.Api.Configuration;
using CanaryDeliveries.Backoffice.Api.PurchaseApplication.Search.Repositories;

namespace CanaryDeliveries.Backoffice.Api.PurchaseApplication.Search.Controllers
{
    public static class Factory
    {
        public static PurchaseApplicationRepository PurchaseApplicationRepository()
        {
            return new PurchaseApplicationEntityFrameworkRepository();
        }
    }
}