using System.Collections.Generic;
using System.Linq;
using CanaryDeliveries.PurchaseApplication.DbContext;
using CanaryDeliveries.PurchaseApplication.Domain;

namespace CanaryDeliveries.PurchaseApplication.Repositories
{
    public interface PurchaseApplicationEntityFrameworkRepository : PurchaseApplicationRepository
    {
        void PurchaseApplicationRepository.Create(Domain.PurchaseApplication purchaseApplication)
        {
            using (var dbContext = new PurchaseApplicationDbContext())
            {
                var state = purchaseApplication.State;
                var dbEntity = new DbContext.PurchaseApplication
                {
                    Id = state.Id.Value,
                    Products = state.Products.Map(product => new Product
                    {
                        Id = "sa",
                        Link = product.Link.Value,
                        Units = product.Units.Value,
                        PromotionCode = product.PromotionCode.MatchUnsafe(None: () => null, Some: x => x.Value),
                        AdditionalInformation = product.AdditionalInformation.MatchUnsafe(None: () => null, Some: x => x.Value)
                    }).ToList(),
                    Client = new Client
                    {
                        Id = "sa",
                        Email = state.Client.Email.Value,
                        Name = state.Client.Name.ToString(),
                        PhoneNumber = state.Client.PhoneNumber.ToString()
                    },
                    AdditionalInformation = state.AdditionalInformation.MatchUnsafe(None: () => null, Some: x => x.Value)
                    //falta el CreationDate
                };
                dbContext.PurchaseApplications.Add(dbEntity);
                dbContext.SaveChanges();
            }
        }
    }
}