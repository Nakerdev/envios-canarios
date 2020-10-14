using NUnit.Framework;

namespace EnviosCanarios.Tests.Unit.WebAppApi.PurchaseApplication.Controllers
{
    public class PurchaseApplicationControllerTests
    {
        private readonly PurchaseApplicationController controller;

        [SetUp]
        public void Setup()
        {
            controller = new PurchaseApplicationController();
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}