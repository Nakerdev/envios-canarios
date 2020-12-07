using Swashbuckle.AspNetCore.Filters;

namespace CanaryDeliveries.Backoffice.Api.Users.Login.Controllers.Documentation
{
    public sealed class LoginRequestExample : IExamplesProvider<LoginController.RequestDto>
    {
        public LoginController.RequestDto GetExamples()
        {
            return new LoginController.RequestDto
            {
                Email = "user@email.com",
                Password = "MiPassSuperSegura"
            };
        }
    }
}