using System.Linq;
using CanaryDeliveries.PurchaseApplication.DbContext;
using CanaryDeliveries.PurchaseApplication.Domain;

namespace CanaryDeliveries.PurchaseApplication.Repositories
{
    public interface PurchaseApplicationEntityFrameworkRepository : PurchaseApplicationRepository
    {
        void PurchaseApplicationRepository.Create(Domain.PurchaseApplication purchaseApplication)
        {
            using var dbContext = new PurchaseApplicationDbContext();
            var dbEntity = BuildDbPurchaseApplication(purchaseApplication);
            dbContext.PurchaseApplications.Add(dbEntity);
            dbContext.SaveChanges();
        }

        private static DbContext.PurchaseApplication BuildDbPurchaseApplication(Domain.PurchaseApplication purchaseApplication)
        {
            var state = purchaseApplication.State;
            return new DbContext.PurchaseApplication
            {
                Id = state.Id.Value,
                Products = state.Products.Map(BuildDbProduct).ToList(),
                Client = BuildDbClient(),
                AdditionalInformation = state.AdditionalInformation.MatchUnsafe(None: () => null, Some: x => x.Value),
                CreationDateTime = state.CreationDateTime
            };

            Product BuildDbProduct(Domain.Entities.Product.PersistenceState product)
            {
                return new Product
                {
                    Id = product.Id.Value,
                    Link = product.Link.Value,
                    Units = product.Units.Value,
                    PromotionCode = product.PromotionCode.MatchUnsafe(None: () => null, Some: x => x.Value),
                    AdditionalInformation = product.AdditionalInformation.MatchUnsafe(None: () => null, Some: x => x.Value)
                };
            }

            Client BuildDbClient()
            {
                return new Client
                {
                    Id = state.Client.Id.Value,
                    Email = state.Client.Email.Value,
                    Name = state.Client.Name.Value,
                    PhoneNumber = state.Client.PhoneNumber.Value
                };
            }
        }
    }
}