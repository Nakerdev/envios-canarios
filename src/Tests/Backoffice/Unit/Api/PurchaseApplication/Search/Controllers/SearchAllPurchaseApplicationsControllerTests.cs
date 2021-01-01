using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CanaryDeliveries.Backoffice.Api.Extensions;
using CanaryDeliveries.Backoffice.Api.PurchaseApplication.Search.Controllers;
using CanaryDeliveries.Backoffice.Api.PurchaseApplication.Search.Repositories;
using CanaryDeliveries.PurchaseApplication.Domain;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PurchaseApplicationRepository = CanaryDeliveries.Backoffice.Api.PurchaseApplication.Search.Repositories.PurchaseApplicationRepository;

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
            responseModel.First().Id.Should().Be(purchaseApplication.Id);
            responseModel.First().Products.Count.Should().Be(purchaseApplication.Products.Count);
            responseModel.First().Products.First().Id.Should().Be(purchaseApplication.Products.First().Id);
            responseModel.First().Products.First().Link.Should().Be(purchaseApplication.Products.First().Link);
            responseModel.First().Products.First().Units.Should().Be(purchaseApplication.Products.First().Units);
            responseModel.First().Products.First().AdditionalInformation.Should()
                .Be(purchaseApplication.Products.First().AdditionalInformation);
            responseModel.First().Products.First().PromotionCode.Should()
                .Be(purchaseApplication.Products.First().PromotionCode);
            responseModel.First().Client.Name.Should().Be(purchaseApplication.Client.Name);
            responseModel.First().Client.PhoneNumber.Should().Be(purchaseApplication.Client.PhoneNumber);
            responseModel.First().Client.Email.Should().Be(purchaseApplication.Client.Email);
            responseModel.First().AdditionalInformation.Should().Be(purchaseApplication.AdditionalInformation);
            responseModel.First().CreationDateTime.Should().Be(purchaseApplication.CreationDateTime.ToISO8601());
            responseModel.First().State.Should().Be(State.PendingOfPayment.ToString());
        }

        private static PurchaseApplicationDto BuildPurchaseApplication()
        {
            return new PurchaseApplicationDto(
                id: "b5cd78a5-2e26-498a-a399-2c5cb2bf0f54",
                products: new ReadOnlyCollection<PurchaseApplicationDto.ProductDto>(
                    new List<PurchaseApplicationDto.ProductDto>
                    {
                        new PurchaseApplicationDto.ProductDto(
                            id: "e2b0a637-54fe-4542-ac2f-b8cba27ab6f8",
                            link: "https://addidas.com/product/1",
                            units: 1,
                            additionalInformation: "Informacion adicional del producto",
                            promotionCode: "ADD-123")
                    }
                ),
                client: new PurchaseApplicationDto.ClientDto(
                    name: "Alfredo",
                    phoneNumber: "610121212",
                    email: "alfredo@emai.com"),
                additionalInformation: "Informacion adicional del pedido",
                creationDateTime: new DateTime(2020, 10, 10, 12, 30, 00),
                state: State.PendingOfPayment);
        }
    }
}