using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using CanaryDeliveries.Backoffice.Api.Auth;
using CanaryDeliveries.Backoffice.Api.Users.Login.Controllers.Documentation;
using CanaryDeliveries.Backoffice.Api.Users.Login.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace CanaryDeliveries.Backoffice.Api.Users.Login.Controllers
{
    [ApiController]
    [Route("/v1/login")]
    public class LoginController : ControllerBase
    {
        private readonly LoginService loginService;

        public LoginController(LoginService loginService)
        {
            this.loginService = loginService;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerOperation(summary: "Authenticate a user")]
        [SwaggerResponse(statusCode: 200, description: "The user credentials are correct. It returns a Json Web Token.")]
        [SwaggerResponseExample(200, typeof(LoginResponseExample))]
        [SwaggerResponse(statusCode: 401, description: "Incorrect user credentials")]
        [SwaggerResponse(statusCode: 500, description: "Unhandled error")]
        [SwaggerRequestExample(typeof(RequestDto), typeof(LoginRequestExample))]
        public ActionResult Execute([FromBody] RequestDto request)
        {
            return loginService.Authenticate(BuildLoginRequest(request))
                .Match(
                    Left: _ => new UnauthorizedResult(),
                    Right: BuildSuccessResponse);
        }

        private static LoginService.LoginRequest BuildLoginRequest(RequestDto request)
        {
            return new LoginService.LoginRequest(
                email: request.Email,
                password: request.Password);
        }
        
        private static ActionResult BuildSuccessResponse(Token token)
        {
            var responseDto = new ResponseDto{Token = token.Value};
            return new OkObjectResult(responseDto);
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
