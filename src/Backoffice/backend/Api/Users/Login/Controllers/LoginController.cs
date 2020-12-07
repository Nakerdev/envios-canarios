using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using CanaryDeliveries.Backoffice.Api.Users.Login.Controllers.Documentation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace CanaryDeliveries.Backoffice.Api.Users.Login.Controllers
{
    [ApiController]
    [Route("/v1/login")]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerOperation(summary: "Authenticate a user")]
        [SwaggerResponse(statusCode: 200, description: "The user credentials are correct. It returns a Json Web Token.")]
        [SwaggerResponseExample(200, typeof(LoginResponseExample))]
        [SwaggerResponse(statusCode: 401, description: "Incorrect user credentials")]
        [SwaggerResponse(statusCode: 500, description: "Unhandled error")]
        [SwaggerRequestExample(typeof(RequestDto), typeof(LoginRequestExample))]
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

        public class ResponseDto
        {
            public string Token { get; set; }
        }
    }
}
