using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;

namespace CanaryDeliveries.Backoffice.Api.PurchaseApplication.Search.Controllers.Documentation
{
    public sealed class PurchaseApplicationsResponseExample : IExamplesProvider<SearchAllPurchaseApplicationsController.PurchaseApplicationDto>
    {
        public SearchAllPurchaseApplicationsController.PurchaseApplicationDto GetExamples()
        {
            return new SearchAllPurchaseApplicationsController.PurchaseApplicationDto(
                id: "b5cd78a5-2e26-498a-a399-2c5cb2bf0f54",
                products: new List<SearchAllPurchaseApplicationsController.PurchaseApplicationDto.ProductDto>
                {
                    new SearchAllPurchaseApplicationsController.PurchaseApplicationDto.ProductDto(
                        id: "e2b0a637-54fe-4542-ac2f-b8cba27ab6f8",
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
                creationDateTime: "2020-10-10T16:33:34.3040300",
                state: "PendingOfPayment");
        }
    }
}