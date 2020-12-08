using System;
using CanaryDeliveries.Backoffice.Api.Auth;
using CanaryDeliveries.Backoffice.Api.Security;
using CanaryDeliveries.Backoffice.Api.Users.Login.Repositories;
using CanaryDeliveries.Backoffice.Api.Users.Login.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace CanaryDeliveries.Tests.Backoffice.Unit.Api.Users.Login.Services
{
    [TestFixture]
    public sealed class LoginServiceTests
    {
        private Mock<BackofficeUserRepository> backofficeUserRepository;
        private Mock<CryptoServiceProvider> cryptoServiceProvider;
        private Mock<TokenHandler> tokenHandler;
        private LoginService loginService;

        [SetUp]
        public void SetUp()
        {
            backofficeUserRepository = new Mock<BackofficeUserRepository>();
            cryptoServiceProvider = new Mock<CryptoServiceProvider>();
            tokenHandler = new Mock<TokenHandler>();
            loginService = new LoginService(
                backofficeUserRepository: backofficeUserRepository.Object,
                cryptoServiceProvider: cryptoServiceProvider.Object, 
                tokenHandler: tokenHandler.Object);
        }
        
        [Test]
        public void AuthenticatesUser()
        {
            var request = BuildRequest();
            var backofficeUser = BuildBackofficeUser(request.Email);
            backofficeUserRepository
                .Setup(x => x.SearchBy(request.Email))
                .Returns(backofficeUser);
           cryptoServiceProvider
               .Setup(x => x.ComputeHash(request.Password))
               .Returns(new Hash(value: backofficeUser.Password));
           var token = new Token(value: "jwt-token");
           tokenHandler
               .Setup(x => x.Create(backofficeUser))
               .Returns(token);
            
            var result = loginService.Authenticate(request);
            
            result.IsRight.Should().BeTrue();
        }
        
        [Test]
        public void DoesNotAuthenticateUserWhenCredentialsAreNotValid()
        {
            backofficeUserRepository
                .Setup(x => x.SearchBy(It.IsAny<string>()))
                .Returns(BuildBackofficeUser());
            cryptoServiceProvider
               .Setup(x => x.ComputeHash(It.IsAny<string>()))
               .Returns(new Hash(value: "another-password-hash"));
            
            var result = loginService.Authenticate(BuildRequest());

            result.IsLeft.Should().BeTrue();
            result.IfLeft(error => error.Should().Be(LoginError.InvalidCredentials));
            tokenHandler
                .Verify(x => x.Create(It.IsAny<BackofficeUser>()), Times.Never);
        }
        
        [Test]
        public void DoesNotAuthenticateUserWhenUserNotFound()
        {
            backofficeUserRepository
                .Setup(x => x.SearchBy(It.IsAny<string>()))
                .Returns((BackofficeUser) null);
            
            var result = loginService.Authenticate(BuildRequest());

            result.IsLeft.Should().BeTrue();
            result.IfLeft(error => error.Should().Be(LoginError.UserNotFound));
            tokenHandler
                .Verify(x => x.Create(It.IsAny<BackofficeUser>()), Times.Never);
        }
        
        private static LoginService.LoginRequest BuildRequest()
        {
            return new LoginService.LoginRequest(
                email: "user@email.com",
                password: "userPassword");
        }

        private static BackofficeUser BuildBackofficeUser(string userEmail = "user@email.com")
        {
            return new BackofficeUser(
                id: new Guid("9bd0af33-87b5-48d6-92a7-7122f71585a5"),
                email: userEmail,
                password: "hashedPassword");
        }
    }
}