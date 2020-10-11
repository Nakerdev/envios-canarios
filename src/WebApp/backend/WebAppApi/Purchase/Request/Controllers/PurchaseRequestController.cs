using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace WebAppApi.Purchase.Request.Controllers
{
    [ApiController]
    [Route("/v1/purchase/request")]
    public class PurchaseRequestController : ControllerBase
    {
        [HttpPost]
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
