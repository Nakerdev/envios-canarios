using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;

namespace WebAppApi.PurchaseApplication.Controllers.Documentation
{
    public sealed class PurchaseApplicationRequestExample : IExamplesProvider<PurchaseApplicationController.PurchaseApplicationRequest>
    {
        public PurchaseApplicationController.PurchaseApplicationRequest GetExamples()
        {
            return new PurchaseApplicationController.PurchaseApplicationRequest
            {
                Products = new List<PurchaseApplicationController.Product>
                {
                    new PurchaseApplicationController.Product
                    {
                        Link = "https://www.adidas.es/zapatilla-zx-2k-4d/FY9089.html",
                        Units = "1",
                        AdditionalInformation = "Color negro, talla 43",
                        PromotionCode = "ADDIDAS-123"
                    }
                },
                Client = new PurchaseApplicationController.Client{
                    Name = "Alfredo",
                    Phone = "123123123",
                    Email = "alfredo@elguapo.com"
                },
                AdditionalInformation = "Si el código promocional no funciona quiero el producto igualmente"
            };
        }
    }
}