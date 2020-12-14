using CanaryDeliveries.PurchaseApplication.Domain.ValueObjects;
using LanguageExt;

namespace CanaryDeliveries.PurchaseApplication.Domain
{
    public interface PurchaseApplicationRepository
    {
        void Create(PurchaseApplication purchaseApplication);
        void Update(PurchaseApplication purchaseApplication);
        Option<PurchaseApplication> SearchBy(Id purchaseApplicationId);
    }
}