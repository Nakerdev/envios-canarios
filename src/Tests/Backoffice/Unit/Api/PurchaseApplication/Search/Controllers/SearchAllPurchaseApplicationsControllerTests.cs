using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CanaryDeliveries.Backoffice.Api.PurchaseApplication.Search.Controllers;
using CanaryDeliveries.Backoffice.Api.PurchaseApplication.Search.Repositories;
using CanaryDeliveries.Backoffice.Api.Utils;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace CanaryDeliveries.Tests.Backoffice.Unit.Api.PurchaseApplication.Search.Controllers
{
    [TestFixture] 
    public class SearchAllPurchaseApplicationsControllerTests
    {
        private Mock<PurchaseApplicationRepository> purchaseApplicationRepository;
        private SearchAllPurchaseApplicationsController controller;

        [SetUp]
        public void SetUp()
        {
            purchaseApplicationRepository = new Mock<PurchaseApplicationRepository>();
            controller = new SearchAllPurchaseApplicationsController(
                purchaseApplicationRepository: purchaseApplicationRepository.Object);
        }
        
        [Test]
        public void SearchesAllPurchaseApplications()
        {
            var purchaseApplication = BuildPurchaseApplication();
            purchaseApplicationRepository
                .Setup(x => x.SearchAll())
                .Returns(new ReadOnlyCollection<PurchaseApplicationDto>(
                    new List<PurchaseApplicationDto>
                    {
                        purchaseApplication
                    }));
            
            var response = controller.Search() as ObjectResult;

            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            var responseModel = (List<SearchAllPurchaseApplicationsController.PurchaseApplicationDto>) response.Value;
            responseModel.Count.Should().Be(1);
            responseModel.First().Products.Count.Should().Be(purchaseApplication.Products.Count);
            responseModel.First().Products.First().Link.Should().Be(purchaseApplication.Products.First().Link);
            responseModel.First().Products.First().Units.Should().Be(purchaseApplication.Products.First().Units);
            responseModel.First().Products.First().AdditionalInformation.Should().Be(purchaseApplication.Products.First().AdditionalInformation);
            responseModel.First().Products.First().PromotionCode.Should().Be(purchaseApplication.Products.First().PromotionCode);
            responseModel.First().Client.Name.Should().Be(purchaseApplication.Client.Name);
            responseModel.First().Client.PhoneNumber.Should().Be(purchaseApplication.Client.PhoneNumber);
            responseModel.First().Client.Email.Should().Be(purchaseApplication.Client.Email);
            responseModel.First().AdditionalInformation.Should().Be(purchaseApplication.AdditionalInformation);
            responseModel.First().CreationDateTime.Should().Be(purchaseApplication.CreationDateTime.ToISO8601());
        }

        private static PurchaseApplicationDto BuildPurchaseApplication()
        {
            return new PurchaseApplicationDto(
                products: new List<PurchaseApplicationDto.ProductDto>
                {
                    new PurchaseApplicationDto.ProductDto(
                        link: "https://addidas.com/product/1",
                        units: 1,
                        additionalInformation: "Informacion adicional del producto",
                        promotionCode: "ADD-123")
                },
                client: new PurchaseApplicationDto.ClientDto(
                    name: "Alfredo",
                    phoneNumber: "610121212",
                    email: "alfredo@emai.com"),
                additionalInformation: "Informacion adicional del pedido",
                creationDateTime: new DateTime(2020, 10, 10, 12, 30, 00));
        }
    }
}
