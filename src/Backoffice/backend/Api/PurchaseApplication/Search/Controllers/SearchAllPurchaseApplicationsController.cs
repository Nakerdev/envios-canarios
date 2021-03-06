using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
using CanaryDeliveries.Backoffice.Api.Extensions;
using CanaryDeliveries.Backoffice.Api.PurchaseApplication.Search.Controllers.Documentation;
using CanaryDeliveries.Backoffice.Api.PurchaseApplication.Search.Repositories;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

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
        [SwaggerOperation(summary: "Search all existing purchase applications", Tags = new []{"Purchase Applications"})]
        [SwaggerResponse(statusCode: 200, description: "Returns the purchase applications found", typeof(PurchaseApplicationDto))]
        [SwaggerResponse(statusCode: 401, description: "Unauthorized request")]
        [SwaggerResponse(statusCode: 500, description: "Unhandled error")]
        [SwaggerResponseExample(statusCode: 200, examplesProviderType: typeof(PurchaseApplicationsResponseExample))]
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
                id: purchaseApplication.Id,
                products: purchaseApplication.Products.Map(BuildResponseProductDto).ToList(),
                client: BuildResponseClient(),
                additionalInformation: purchaseApplication.AdditionalInformation,
                creationDateTime: purchaseApplication.CreationDateTime.ToISO8601(),
                state: purchaseApplication.State.ToString(),
                rejection: purchaseApplication.Rejection.Map(BuildRejectionDto).IfNoneUnsafe(() => null));

            PurchaseApplicationDto.ProductDto BuildResponseProductDto(
                Repositories.PurchaseApplicationDto.ProductDto product)
            {
                return new PurchaseApplicationDto.ProductDto(
                    id: product.Id,
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
            
            PurchaseApplicationDto.RejectionDto BuildRejectionDto(
                Repositories.PurchaseApplicationDto.RejectionDto rejection)
            {
                return new PurchaseApplicationDto.RejectionDto(
                    dateTime: rejection.DateTime.ToISO8601(),
                    reason: rejection.Reason);
            }
        }
        
        public sealed class PurchaseApplicationDto
        {
            [SwaggerSchema("The purchase application identifier")] 
            [Required]            
            public string Id { get; }
            
            [SwaggerSchema("List of products that the client want to purchase")] 
            [Required]            
            public List<ProductDto> Products { get; }
            
            [SwaggerSchema("The client information")]            
            [Required]            
            public ClientDto Client { get; }
            
            [SwaggerSchema("General additional information of the application")]            
            public string AdditionalInformation { get; }
            
            [SwaggerSchema("The creation date in ISO8601 format")]            
            [Required]            
            public string CreationDateTime { get; }

            [SwaggerSchema("The purchase application state, it can be: PendingOfPayment || Rejected ")]            
            [Required]            
            public string State { get; }

            [SwaggerSchema("The purchase application rejection. It is optional.")]            
            public RejectionDto Rejection { get; }

            public PurchaseApplicationDto(
                string id,
                List<ProductDto> products,
                ClientDto client,
                string additionalInformation,
                string creationDateTime, 
                string state, 
                RejectionDto rejection)
            {
                Id = id;
                Products = products;
                Client = client;
                AdditionalInformation = additionalInformation;
                CreationDateTime = creationDateTime;
                State = state;
                Rejection = rejection;
            }

            public sealed class ProductDto
            {
                [SwaggerSchema("The product indentifier")]            
                [Required]            
                public string Id { get; }
                
                [SwaggerSchema("The link of the product that the client want to purchase")]            
                [Required]            
                public string Link { get; }
                
                [SwaggerSchema("Number of product units to purchase")]            
                [Required]            
                public int Units { get; }
                
                [SwaggerSchema("Additional information needed to purchase the product, like size, color, etc.")]            
                public string AdditionalInformation { get; }
                
                [SwaggerSchema("The promotional code to apply in the product purchase")]
                public string PromotionCode { get; }

                public ProductDto(
                    string id,
                    string link,
                    int units,
                    string additionalInformation,
                    string promotionCode)
                {
                    Id = id;
                    Link = link;
                    Units = units;
                    AdditionalInformation = additionalInformation;
                    PromotionCode = promotionCode;
                }
            }

            public sealed class ClientDto
            {
                [SwaggerSchema("The client name")]            
                [Required]            
                public string Name { get; }
                
                [SwaggerSchema("The client phone number")]            
                [Required]            
                public string PhoneNumber { get; }
                
                [SwaggerSchema("The client email")]            
                [Required]            
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
            
            public sealed class RejectionDto
            {
                [SwaggerSchema("The rejection date time in ISO8601 format")]            
                [Required]            
                public string DateTime { get; }
                
                [SwaggerSchema("The rejection reason")]            
                [Required]            
                public string Reason { get; }

                public RejectionDto(
                    string dateTime, 
                    string reason)
                {
                    DateTime = dateTime;
                    Reason = reason;
                }
            }
        }
    }
}