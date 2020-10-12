using System.Net.Mime;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebAppApi.Utils;

namespace WebAppApi.PurchaseApplication.Controllers
{
    [ApiController]
    [Route("/v1/purchase-application")]
    public class PurchaseApplicationController : ControllerBase
    {
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerOperation(summary: "Creates a purchase application")]
        [SwaggerResponse(statusCode: 200, description: "The purchase application was created successfuly")]
        [SwaggerResponse(statusCode: 404, description: "The purchase application request has validation errors", type: typeof(BadRequestResponseModel<PurchaseApplicationCreationRequestErrorCode>))]
        [SwaggerResponse(statusCode: 500, description: "Unhandled error")]
        public ActionResult Execute([FromBody] PurchaseApplicationRequest request)
        {
            return Ok();
        }

        public sealed class PurchaseApplicationRequest
        {
            [SwaggerSchema("List of products that the client want to purchase")]            
            [RequiredAttribute]            
            public List<Product> Products { get; set; }

            [SwaggerSchema("The client information")]            
            [RequiredAttribute]            
            public Client Client { get; set; }

            [SwaggerSchema("General additional information that the client want to specifies")]            
            public string AdditionalInformation { get; set; }
        }

        public sealed class Product 
        {
            [SwaggerSchema("The link of the product that the client want to purchase", Format = "URL")]            
            [RequiredAttribute]            
            public string Link { get; set; }

            [SwaggerSchema("Number of product units to purchase", Format = "Integer")]            
            [RequiredAttribute]            
            public string Units { get; set; }

            [SwaggerSchema("Additional information needed to purchas the product, like size, color, etc.")]            
            public string AdditionalInformation { get; set; }

            [SwaggerSchema("The promotional code to apply in the product purchase")]            
            public string PromotionCode { get; set; }
        }

        public sealed class Client 
        {
            [SwaggerSchema("The client name")]            
            [RequiredAttribute]            
            public string Name { get; set; }

            [SwaggerSchema("The client telephone number", Format = "Integer")]            
            [RequiredAttribute]            
            public string TelephoneNumber { get; set; }

            [SwaggerSchema("The client email", Format = "Email address")]            
            [RequiredAttribute]            
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
