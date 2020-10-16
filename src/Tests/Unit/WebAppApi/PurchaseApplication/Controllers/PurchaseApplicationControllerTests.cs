using System.ComponentModel;
using NUnit.Framework;
using WebAppApi.PurchaseApplication.Controllers;

namespace EnviosCanarios.Tests.Unit.WebAppApi.PurchaseApplication.Controllers
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
            var request = new PurchaseApplicationController.PurchaseApplicationRequest
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

            var response = controller.Execute(request);

            false.Should().BeTrue();
        }
    }
}