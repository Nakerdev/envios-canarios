using Swashbuckle.AspNetCore.Filters;

namespace CanaryDeliveries.Backoffice.Api.Users.Login.Controllers.Documentation
{
    public sealed class LoginResponseExample : IExamplesProvider<LoginController.ResponseDto>
    {
        public LoginController.ResponseDto GetExamples()
        {
            return new LoginController.ResponseDto
            {
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VyRGF0YSI6IntcIlVzZXJJZFwiOlwiY2Y4ZjRhY2QtZTdl"
            };
        }
    }
}