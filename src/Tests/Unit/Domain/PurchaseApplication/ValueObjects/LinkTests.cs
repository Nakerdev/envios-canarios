using CanaryDeliveries.Domain.PurchaseApplication.ValueObjects;
using FluentAssertions;
using NUnit.Framework;
using static LanguageExt.Prelude;

namespace CanaryDeliveries.Tests.Domain.PurchaseApplication.ValueObjects
{
    [TestFixture]
    public sealed class LinkTests
    {
        [Test]
        public void CreatesALink()
        {
            const string link = "https://adidas.com/product/0";
            
            var result = Link.Create(link);

            result.IsSuccess.Should().BeTrue();
            result.IfSuccess(createdLink =>
            {
                var expectedLink = new Link(link);
                createdLink.Should().Be(expectedLink);
            });
        }
        
        [Test]
        public void DoesNotCreateALinkWhenValueIsEmpty()
        {
            var result = Link.Create(None);

            result.IsFail.Should().BeTrue();
            result.IfFail(validationErrors =>
            {
                validationErrors.Count.Should().Be(1);
                validationErrors.First().FieldId.Should().Be(nameof(Link));
                validationErrors.First().ErrorCode.Should().Be(LinkValidationError.Required);
            });
        }
    }
}