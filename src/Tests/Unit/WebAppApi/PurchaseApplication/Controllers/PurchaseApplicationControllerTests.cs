using System.Collections.Generic;
using CanaryDeliveries.WebApp.Api.PurchaseApplication.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace CanaryDeliveries.Tests.Unit.WebApp.Api.PurchaseApplication.Controllers
{
    public class PurchaseApplicationControllerTests
    {
        private PurchaseApplicationController controller;

        [SetUp]
        public void Setup()
        {
            controller = new PurchaseApplicationController();
        }

        [Test]
        public void CreatesPurchaseApplication()
        {
            var request = BuildPurchaseApplicationRequest();

            var response = controller.Execute(request) as StatusCodeResult;

            response.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        private static PurchaseApplicationController.PurchaseApplicationRequest BuildPurchaseApplicationRequest()
        {
            return new PurchaseApplicationController.PurchaseApplicationRequest
            {
                Products = new List<PurchaseApplicationController.Product>
                {
                    new PurchaseApplicationController.Product
                    {
                        Link = "https://www.addida.com/any/product",
                        Units = "1",
                        AdditionalInformation = "Additional product information",
                        PromotionCode = "ADDIDAS-123"
                    }
                },
                Client = new  PurchaseApplicationController.Client
                {
                    Name = "Alfredo",
                    Phone = "123123123",
                    Email = "alfredo@elguapo.com"
                },
                AdditionalInformation = "Additional purchase application information"
            };
        }
    }
}