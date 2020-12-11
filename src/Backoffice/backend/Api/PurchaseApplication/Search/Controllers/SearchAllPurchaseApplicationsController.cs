using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using CanaryDeliveries.Backoffice.Api.PurchaseApplication.Search.Repositories;
using CanaryDeliveries.Backoffice.Api.Utils;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CanaryDeliveries.Backoffice.Api.PurchaseApplication.Search.Controllers
{
    [ApiController]
    [Route("/v1/purchase-applications")]
    public class SearchAllPurchaseApplicationsController : ControllerBase
    {
        private readonly PurchaseApplicationRepository purchaseApplicationRepository;

        public SearchAllPurchaseApplicationsController(PurchaseApplicationRepository purchaseApplicationRepository)
        {
            this.purchaseApplicationRepository = purchaseApplicationRepository;
        }

        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerOperation(summary: "Search all existing purchase applications")]
        [SwaggerResponse(statusCode: 200, description: "Returns the purchase applications found",
            typeof(PurchaseApplicationDto))]
        [SwaggerResponse(statusCode: 401, description: "Unauthorized request")]
        [SwaggerResponse(statusCode: 500, description: "Unhandled error")]
        //[SwaggerRequestExample(typeof(RequestDto), typeof(LoginRequestExample))]
        //[SwaggerResponseExample(statusCode: 200, examplesProviderType: typeof(LoginResponseExample))]
        public ActionResult Search()
        {
            var purchaseApplications = purchaseApplicationRepository
                .SearchAll()
                .Map(BuildResponsePurchaseApplicationDto)
                .ToList();
            return new OkObjectResult(purchaseApplications);
        }

        private static PurchaseApplicationDto BuildResponsePurchaseApplicationDto(
            Repositories.PurchaseApplicationDto purchaseApplication)
        {
            return new PurchaseApplicationDto(
                products: purchaseApplication.Products.Map(BuildResponseProductDto).ToList(),
                client: BuildResponseClient(),
                additionalInformation: purchaseApplication.AdditionalInformation,
                creationDateTime: purchaseApplication.CreationDateTime.ToISO8601());

            PurchaseApplicationDto.ProductDto BuildResponseProductDto(
                Repositories.PurchaseApplicationDto.ProductDto product)
            {
                return new PurchaseApplicationDto.ProductDto(
                    link: product.Link,
                    units: product.Units,
                    additionalInformation: product.AdditionalInformation,
                    promotionCode: product.PromotionCode);
            }

            PurchaseApplicationDto.ClientDto BuildResponseClient()
            {
                return new PurchaseApplicationDto.ClientDto(
                    name: purchaseApplication.Client.Name,
                    phoneNumber: purchaseApplication.Client.PhoneNumber,
                    email: purchaseApplication.Client.Email);
            }
        }

        public class PurchaseApplicationDto
        {
            public List<ProductDto> Products { get; }
            public ClientDto Client { get; }
            public string AdditionalInformation { get; }
            public string CreationDateTime { get; }

            public PurchaseApplicationDto(
                List<ProductDto> products,
                ClientDto client,
                string additionalInformation,
                string creationDateTime)
            {
                Products = products;
                Client = client;
                AdditionalInformation = additionalInformation;
                CreationDateTime = creationDateTime;
            }

            public sealed class ProductDto
            {
                public string Link { get; }
                public int Units { get; }
                public string AdditionalInformation { get; }
                public string PromotionCode { get; }

                public ProductDto(
                    string link,
                    int units,
                    string additionalInformation,
                    string promotionCode)
                {
                    Link = link;
                    Units = units;
                    AdditionalInformation = additionalInformation;
                    PromotionCode = promotionCode;
                }
            }

            public sealed class ClientDto
            {
                public string Name { get; }
                public string PhoneNumber { get; }
                public string Email { get; }

                public ClientDto(
                    string name,
                    string phoneNumber,
                    string email)
                {
                    Name = name;
                    PhoneNumber = phoneNumber;
                    Email = email;
                }
            }
        }
    }
}