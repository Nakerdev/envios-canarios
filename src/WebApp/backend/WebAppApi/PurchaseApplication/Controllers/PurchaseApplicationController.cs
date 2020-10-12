using System;
using System.Net.Mime;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using WebAppApi.Utils;
using WebAppApi.PurchaseApplication.Controllers.Documentation;

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
        [SwaggerResponse(statusCode: 404, description: "The purchase application request has validation errors. It response never returns an operation error", type: typeof(BadRequestResponseModel<PurchaseApplicationCreationRequestErrorCode>))]
        [SwaggerResponse(statusCode: 500, description: "Unhandled error")]
        [SwaggerRequestExample(typeof(PurchaseApplicationRequest), typeof(PurchaseApplicationRequestExample))]
        [SwaggerResponseExample(400, typeof(BadRequestResponseModelExampleForValidationsError))]
        public ActionResult Execute()
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

            [SwaggerSchema("General additional information that the client want to specifies", Description = "Must contains a maximum of 1000 characters")]            
            public string AdditionalInformation { get; set; }
        }

        public sealed class Product 
        {
            [SwaggerSchema("The link of the product that the client want to purchase", Description = "Must be a valid URL and contains a maximum of 1000 characters")]            
            [RequiredAttribute]            
            public string Link { get; set; }

            [SwaggerSchema("Number of product units to purchase", Description = "Must be a numeric value greather than 0")]            
            [RequiredAttribute]            
            public string Units { get; set; }

            [SwaggerSchema("Additional information needed to purchas the product, like size, color, etc.", Description = "Must contains a maximum of 1000 characters")]            
            public string AdditionalInformation { get; set; }

            [SwaggerSchema("The promotional code to apply in the product purchase",  Description = "Must contains a maximum of 50 characters")]
            public string PromotionCode { get; set; }
        }

        public sealed class Client 
        {
            [SwaggerSchema("The client name", Description = "Must contains a maximum of 255 characters")]            
            [RequiredAttribute]            
            public string Name { get; set; }

            [SwaggerSchema("The client phone number", Description = "Must be a numeric value and contains a maximum of 15 characters")]            
            [RequiredAttribute]            
            public string Phone { get; set; }

            [SwaggerSchema("The client email",Description = "Must be a valid email format and contains a maximum of 255 characters")]            
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
