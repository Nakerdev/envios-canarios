using NUnit.Framework;

namespace CanaryDeliveries.Tests.Backoffice.Unit.Api.Users.Login.Controllers
{
    [TestFixture] 
    public class LoginControllerTests
    {
        /*[Test]
        public void AuthorizesUser()
        {
            var loginService = new Mock<LoginService>();
            var tokenHandler = new Mock<TokenHandler>();
            var controller = new LoginController(
                loginService: loginService.Object,
                tokenHandler: tokenHandler.Object);
            var request = new LoginController.RequestDto
            {
                Email = "user@email.com",
                Password = "MiPassSuperSegura"
            };
            loginService
                .Setup(x => x.AreValidCredentials(It.Is<LoginService.LoginRequest>(y =>
                    y.Email == request.Email
                    && y.Password == request.Password)))
                .Returns(true);
            tokenHandler
                .Setup(x => x.Create())
                
            
            controller.Execute(request);
        }*/
    }
}
