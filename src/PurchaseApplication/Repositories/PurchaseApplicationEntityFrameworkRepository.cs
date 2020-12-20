using System.Collections.Generic;
using System.Linq;
using CanaryDeliveries.PurchaseApplication.DbContext;
using CanaryDeliveries.PurchaseApplication.Domain;
using CanaryDeliveries.PurchaseApplication.Domain.ValueObjects;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.CompilerServices;

namespace CanaryDeliveries.PurchaseApplication.Repositories
{
    public class PurchaseApplicationEntityFrameworkRepository : PurchaseApplicationRepository
    {
        private readonly string purchaseApplicationDbConnectionString;

        public PurchaseApplicationEntityFrameworkRepository(string purchaseApplicationDbConnectionString)
        {
            this.purchaseApplicationDbConnectionString = purchaseApplicationDbConnectionString;
        }

        void PurchaseApplicationRepository.Create(Domain.PurchaseApplication purchaseApplication)
        {
            using var dbContext = new PurchaseApplicationDbContext(purchaseApplicationDbConnectionString);
            var dbEntity = BuildDbPurchaseApplication(purchaseApplication);
            dbContext.PurchaseApplications.Add(dbEntity);
            dbContext.SaveChanges();
        }

        public void Update(Domain.PurchaseApplication purchaseApplication)
        {
            throw new System.NotImplementedException();
        }

        public Option<Domain.PurchaseApplication> SearchBy(Id purchaseApplicationId)
        {
            using var dbContext = new PurchaseApplicationDbContext(purchaseApplicationDbConnectionString);
            var id = purchaseApplicationId.State.Value;
            return dbContext.PurchaseApplications
                .Include(x => x.Products)
                .Include(x => x.Client)
                .Where(x => x.Id == id)
                .ToList()
                .Select(BuildPurchaseApplication)
                .FirstOrDefault();
        }

        private static DbContext.PurchaseApplication BuildDbPurchaseApplication(Domain.PurchaseApplication purchaseApplication)
        {
            var state = purchaseApplication.PersistenceState;
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
        
        private static Domain.PurchaseApplication BuildPurchaseApplication(DbContext.PurchaseApplication dbEntity)
        {
            var persistenceState = new Domain.PurchaseApplication.PersistenceStateDto(
                id: new Id.PersistenceState(dbEntity.Id),
                products: dbEntity.Products.Map(BuildProductPersistenceState).ToList(),
                client: BuildClientPersistenceState(),
                additionalInformation: dbEntity.AdditionalInformation != null
                    ? new AdditionalInformation.PersistenceState(dbEntity.AdditionalInformation) 
                    : Option<AdditionalInformation.PersistenceState>.None,
                creationDateTime: dbEntity.CreationDateTime,
                rejection: null);
            return new Domain.PurchaseApplication(persistenceState);

            Domain.Entities.Product.PersistenceState BuildProductPersistenceState(Product product)
            {
                return new Domain.Entities.Product.PersistenceState(
                    id: new Id.PersistenceState(product.Id),
                    link: new Link.PersistenceState(product.Link),
                    units: new Units.PersistenceState(product.Units),
                    additionalInformation: product.AdditionalInformation != null
                        ? new AdditionalInformation.PersistenceState(product.AdditionalInformation)
                        : Option<AdditionalInformation.PersistenceState>.None,
                    promotionCode: product.PromotionCode != null ?
                        new PromotionCode.PersistenceState(product.PromotionCode)
                        : Option<PromotionCode.PersistenceState>.None);
            }

            Domain.Entities.Client.PersistenceState BuildClientPersistenceState()
            {
                return new Domain.Entities.Client.PersistenceState(
                    id: new Id.PersistenceState(dbEntity.Client.Id),
                    name: new Name.PersistenceState(dbEntity.Client.Name),
                    phoneNumber: new PhoneNumber.PersistenceState(dbEntity.Client.PhoneNumber),
                    email: new Email.PersistenceState(dbEntity.Client.Email));
            }
        }
    }
}