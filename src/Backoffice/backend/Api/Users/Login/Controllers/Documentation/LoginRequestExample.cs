using Swashbuckle.AspNetCore.Filters;

namespace CanaryDeliveries.Backoffice.Api.Users.Login.Controllers.Documentation
{
    public sealed class LoginRequestExample : IExamplesProvider<LoginController.LoginRequestDto>
    {
        public LoginController.LoginRequestDto GetExamples()
        {
            return new LoginController.LoginRequestDto
            {
                Email = "user@email.com",
                Password = "MiPassSuperSegura"
            };
        }
    }
}