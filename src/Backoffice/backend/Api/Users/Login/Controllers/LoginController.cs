using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CanaryDeliveries.Backoffice.Api.Users.Login.Controllers
{
    [ApiController]
    [Route("/v1/login")]
    public class PurchaseApplicationController : ControllerBase
    {
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerOperation(summary: "Authenticate a user")]
        [SwaggerResponse(statusCode: 200, description: "The user credentials are correct. It returns a Json Web Token.")]
        [SwaggerResponse(statusCode: 401, description: "Incorrect user credentials")]
        [SwaggerResponse(statusCode: 500, description: "Unhandled error")]
        //[SwaggerRequestExample(typeof(PurchaseApplicationCreationRequest), typeof(PurchaseApplicationRequestExample))]
        public ActionResult Execute([FromBody] RequestDto creationRequest)
        {
            return Ok();
        }
        
        public class RequestDto
        {
            [SwaggerSchema("User email")]            
            [Required]             
            public string Email { get; set; }
            
            [SwaggerSchema("User password")]            
            [Required]             
            public string Password { get; set; }
        }
    }
}
