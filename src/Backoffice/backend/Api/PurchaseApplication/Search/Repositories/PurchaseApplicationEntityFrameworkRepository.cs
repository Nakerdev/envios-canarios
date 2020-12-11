using System.Collections.ObjectModel;
using System.Linq;
using CanaryDeliveries.PurchaseApplication.DbContext;
using Microsoft.EntityFrameworkCore;

namespace CanaryDeliveries.Backoffice.Api.PurchaseApplication.Search.Repositories
{
    public sealed class PurchaseApplicationEntityFrameworkRepository : PurchaseApplicationRepository
    {
        private readonly string purchaseApplicationDbContextConnectionString;

        public PurchaseApplicationEntityFrameworkRepository(string purchaseApplicationDbContextConnectionString)
        {
            this.purchaseApplicationDbContextConnectionString = purchaseApplicationDbContextConnectionString;
        }

        public ReadOnlyCollection<PurchaseApplicationDto> SearchAll()
        {
            using var dbContext = new PurchaseApplicationDbContext(purchaseApplicationDbContextConnectionString);
            return dbContext.PurchaseApplications
                .Include(x => x.Products)
                .Include(x => x.Client)
                .ToList()
                .Select(BuildPurchaseApplicationDto)
                .ToList()
                .AsReadOnly();
        }

        private static PurchaseApplicationDto BuildPurchaseApplicationDto(
            CanaryDeliveries.PurchaseApplication.DbContext.PurchaseApplication purchaseApplication)
        {
            return new PurchaseApplicationDto(
                products: purchaseApplication.Products.Map(BuildProductDto).ToList().AsReadOnly(),
                client: BuildClientDto(),
                additionalInformation: purchaseApplication.AdditionalInformation,
                creationDateTime: purchaseApplication.CreationDateTime);
            
            PurchaseApplicationDto.ProductDto BuildProductDto(Product product)
            {
                return new PurchaseApplicationDto.ProductDto(
                    link: product.Link,
                    units: product.Units,
                    additionalInformation: product.AdditionalInformation,
                    promotionCode: product.AdditionalInformation);
            }
            
            PurchaseApplicationDto.ClientDto BuildClientDto()
            {
                return new PurchaseApplicationDto.ClientDto(
                    name: purchaseApplication.Client.Name,
                    phoneNumber: purchaseApplication.Client.PhoneNumber,
                    email: purchaseApplication.Client.Email);
            }
        }
    }
}