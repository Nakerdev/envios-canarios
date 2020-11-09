using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
using CanaryDeliveries.PurchaseApplication.Domain.Create;
using CanaryDeliveries.PurchaseApplication.Domain.ValueObjects;
using CanaryDeliveries.WebApp.Api.PurchaseApplication.Controllers.Documentation;
using CanaryDeliveries.WebApp.Api.Utils;
using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace CanaryDeliveries.WebApp.Api.PurchaseApplication.Controllers
{
    [ApiController]
    [Route("/v1/purchase-application")]
    public class PurchaseApplicationController : ControllerBase
    {
        private readonly CreatePurchaseApplicationCommandHandler commandHandler;

        public PurchaseApplicationController(CreatePurchaseApplicationCommandHandler commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerOperation(summary: "Creates a purchase application")]
        [SwaggerResponse(statusCode: 200, description: "The purchase application was created successfully")]
        [SwaggerResponse(statusCode: 404, description: "The purchase application request has validation errors. It response never returns an operation error", type: typeof(BadRequestResponseModel<PurchaseApplicationCreationRequestErrorCode>))]
        [SwaggerResponse(statusCode: 500, description: "Unhandled error")]
        [SwaggerRequestExample(typeof(PurchaseApplicationRequest), typeof(PurchaseApplicationRequestExample))]
        [SwaggerResponseExample(400, typeof(BadRequestResponseModelExampleForValidationsError))]
        public ActionResult Execute([FromBody] PurchaseApplicationRequest request)
        {
            var command = BuildCreatePurchaseApplicationCommand(request);
            return command.Match(
                Fail: errors => throw new NotImplementedException(),
                Succ: comm =>
                {
                    commandHandler.Create(comm);
                    return Ok();
                });
        }

        private static Validation<
            CanaryDeliveries.PurchaseApplication.Domain.ValueObjects.ValidationError<GenericValidationErrorCode>, 
            CreatePurchaseApplicationCommand> BuildCreatePurchaseApplicationCommand(PurchaseApplicationRequest request)
        {
            var commandDto = new CreatePurchaseApplicationCommand.Dto(
                products: request.Products.Map(product =>
                    new CanaryDeliveries.PurchaseApplication.Domain.Entities.Product.Dto(
                        link: product.Link,
                        units: product.Units,
                        additionalInformation: product.AdditionalInformation,
                        promotionCode: product.PromotionCode)).ToList(),
                client: new CanaryDeliveries.PurchaseApplication.Domain.Entities.Client.Dto(
                    name: request.Client.Name,
                    phoneNumber: request.Client.PhoneNumber,
                    email: request.Client.Email),
                additionalInformation: request.AdditionalInformation);
            return CreatePurchaseApplicationCommand.Create(commandDto);
        }

        public sealed class PurchaseApplicationRequest
        {
            [SwaggerSchema("List of products that the client want to purchase")]            
            [Required]            
            public List<Product> Products { get; set; }

            [SwaggerSchema("The client information")]            
            [Required]            
            public Client Client { get; set; }

            [SwaggerSchema("General additional information that the client want to specifies", Description = "Must contains a maximum of 1000 characters")]            
            public string AdditionalInformation { get; set; }
        }

        public sealed class Product 
        {
            [SwaggerSchema("The link of the product that the client want to purchase", Description = "Must be a valid URL and contains a maximum of 1000 characters")]            
            [Required]            
            public string Link { get; set; }

            [SwaggerSchema("Number of product units to purchase", Description = "Must be a numeric value greather than 0")]            
            [Required]            
            public string Units { get; set; }

            [SwaggerSchema("Additional information needed to purchas the product, like size, color, etc.", Description = "Must contains a maximum of 1000 characters")]            
            public string AdditionalInformation { get; set; }

            [SwaggerSchema("The promotional code to apply in the product purchase",  Description = "Must contains a maximum of 50 characters")]
            public string PromotionCode { get; set; }
        }

        public sealed class Client 
        {
            [SwaggerSchema("The client name", Description = "Must contains a maximum of 255 characters")]            
            [Required]            
            public string Name { get; set; }

            [SwaggerSchema("The client phone number", Description = "Must be a numeric value and contains a maximum of 15 characters")]            
            [Required]            
            public string PhoneNumber { get; set; }

            [SwaggerSchema("The client email",Description = "Must be a valid email format and contains a maximum of 255 characters")]            
            [Required]            
            public string Email { get; set; }
        }
    }

    public enum PurchaseApplicationCreationRequestErrorCode 
    {
        Required,
        InvalidFormat,
        WrongLength,
        InvalidValue
    }
}
