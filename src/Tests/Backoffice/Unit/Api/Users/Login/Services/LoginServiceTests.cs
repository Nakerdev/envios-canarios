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
        [Test]
        public void ValidatesCredentials()
        {
            var backofficeUserRepository = new Mock<BackofficeUserRepository>();
            var cryptoServiceProvider = new Mock<CryptoServiceProvider>();
            var loginService = new LoginService(
                backofficeUserRepository: backofficeUserRepository.Object,
                cryptoServiceProvider: cryptoServiceProvider.Object);
            var request = new LoginService.LoginRequest(
                email: "user@email.com",
                password: "userPassword");
            var backofficeUser = new BackofficeUser(
                id: new Guid("9bd0af33-87b5-48d6-92a7-7122f71585a5"),
                email: request.Email,
                password: "hashedPassword");
            backofficeUserRepository
                .Setup(x => x.SearchBy(request.Email))
                .Returns(backofficeUser);
           cryptoServiceProvider
               .Setup(x => x.ComputeHash(request.Password))
               .Returns(new Hash(value: backofficeUser.Password));
            
            var areValidCredentials = loginService.AreValidCredentials(request);

            areValidCredentials.Should().BeTrue();
        }
    }
}