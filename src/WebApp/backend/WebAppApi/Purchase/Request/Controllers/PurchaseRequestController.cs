using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAppApi.Purchase.Request.Controllers
{
    [ApiController]
    [Route("/v1/purchase/request")]
    public class PurchaseRequestController : ControllerBase
    {
        [HttpPost]
        [SwaggerOperation(summary: "Creates a purchase request")]
        [SwaggerResponse(statusCode: 200, description: "The purchase request was created successfuly")]
        [SwaggerResponse(statusCode: 500, description: "Unhandled error")]
        public ActionResult Execute([FromBody] RequestDto request)
        {
            return Ok();
        }

        public sealed class RequestDto
        {
            public List<ProductDto> Products { get; set; }
            public ClientDto Client { get; set; }
            public string AdditionalInformation { get; set; }
            
            public sealed class ProductDto 
            {
                public string Link { get; set; }
                public string Units { get; set; }
                public string AdditionalInformation { get; set; }
                public string PromotionCode { get; set; }
            }

            public sealed class ClientDto 
            {
                public string Name { get; set; }
                public string TelephoneNumber { get; set; }
                public string Email { get; set; }
            }
        }
    }
}
