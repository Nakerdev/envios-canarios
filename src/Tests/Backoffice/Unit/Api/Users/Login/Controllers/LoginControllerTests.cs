using CanaryDeliveries.Backoffice.Api.Auth;
using CanaryDeliveries.Backoffice.Api.Security;
using CanaryDeliveries.Backoffice.Api.Users.Login.Controllers;
using CanaryDeliveries.Backoffice.Api.Users.Login.Repositories;
using CanaryDeliveries.Backoffice.Api.Users.Login.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace CanaryDeliveries.Tests.Backoffice.Unit.Api.Users.Login.Controllers
{
    [TestFixture] 
    public class LoginControllerTests
    {
        private Mock<LoginService> loginService;
        private LoginController controller;

        [SetUp]
        public void SetUp()
        {
            loginService = new Mock<LoginService>(
                It.IsAny<BackofficeUserRepository>(),
                It.IsAny<CryptoServiceProvider>(),
                It.IsAny<TokenHandler>());
            controller = new LoginController(loginService: loginService.Object);
        }
        
        [Test]
        public void AuthorizesUser()
        {
            var request = BuildRequest();
            var token = new Token(value: "jwt-token");
            loginService
                .Setup(x => x.Authenticate(It.Is<LoginService.LoginRequest>(y =>
                    y.Email == request.Email
                    && y.Password == request.Password)))
                .Returns(token);
            
            var response = controller.Execute(request) as ObjectResult;

            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            var responseModel = (LoginController.ResponseDto) response.Value;
            responseModel.Token.Should().Be(token.Value);
        }

        private static LoginController.RequestDto BuildRequest()
        {
            return new LoginController.RequestDto
            {
                Email = "user@email.com",
                Password = "MiPassSuperSegura"
            };
        }
    }
}
