using System;
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
        private LoginService loginService;

        [SetUp]
        public void SetUp()
        {
            backofficeUserRepository = new Mock<BackofficeUserRepository>();
            cryptoServiceProvider = new Mock<CryptoServiceProvider>();
            loginService = new LoginService(
                backofficeUserRepository: backofficeUserRepository.Object,
                cryptoServiceProvider: cryptoServiceProvider.Object);
        }
        
        [Test]
        public void ValidatesCredentials()
        {
            var request = BuildRequest();
            var backofficeUser = BuildBackofficeUser(request.Email);
            backofficeUserRepository
                .Setup(x => x.SearchBy(request.Email))
                .Returns(backofficeUser);
           cryptoServiceProvider
               .Setup(x => x.ComputeHash(request.Password))
               .Returns(new Hash(value: backofficeUser.Password));
            
            var areValidCredentials = loginService.AreValidCredentials(request);

            areValidCredentials.Should().BeTrue();
        }
        
        private static LoginService.LoginRequest BuildRequest()
        {
            return new LoginService.LoginRequest(
                email: "user@email.com",
                password: "userPassword");
        }

        private static BackofficeUser BuildBackofficeUser(string userEmail)
        {
            return new BackofficeUser(
                id: new Guid("9bd0af33-87b5-48d6-92a7-7122f71585a5"),
                email: userEmail,
                password: "hashedPassword");
        }
    }
}