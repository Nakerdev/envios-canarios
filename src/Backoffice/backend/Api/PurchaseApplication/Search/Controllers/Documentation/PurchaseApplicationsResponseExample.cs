using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;

namespace CanaryDeliveries.Backoffice.Api.PurchaseApplication.Search.Controllers.Documentation
{
    public sealed class PurchaseApplicationsResponseExample : IExamplesProvider<SearchAllPurchaseApplicationsController.PurchaseApplicationDto>
    {
        public SearchAllPurchaseApplicationsController.PurchaseApplicationDto GetExamples()
        {
            return new SearchAllPurchaseApplicationsController.PurchaseApplicationDto(
                products: new List<SearchAllPurchaseApplicationsController.PurchaseApplicationDto.ProductDto>
                {
                    new SearchAllPurchaseApplicationsController.PurchaseApplicationDto.ProductDto(
                        link: "https://addidas.com/product/1",
                        units: 1,
                        additionalInformation: "Informacion adicional del producto",
                        promotionCode: "ADD-123")
                },
                client: new SearchAllPurchaseApplicationsController.PurchaseApplicationDto.ClientDto(
                    name: "Alfredo",
                    phoneNumber: "610121212",
                    email: "alfredo@emai.com"),
                additionalInformation: "Informacion adicional del pedido",
                creationDateTime: "2020-10-10T16:33:34.3040300");
        }
    }
}