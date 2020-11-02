using System.Collections.Generic;
using CanaryDeliveries.Domain.PurchaseApplication.ValueObjects;
using CanaryDeliveries.Tests.Domain.PurchaseApplication.Utils;
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
        }
        
        private readonly IReadOnlyList<ValidationErrorTestCase<LinkValidationErrorCode, Link>> _validationErrorTestCases =
            new List<ValidationErrorTestCase<LinkValidationErrorCode, Link>>
            {
                new ValidationErrorTestCase<LinkValidationErrorCode, Link>(
                    builder: () => Link.Create(None),
                    expectedFieldId: nameof(Link),
                    expectedErrorCode: LinkValidationErrorCode.Required),
                new ValidationErrorTestCase<LinkValidationErrorCode, Link>(
                    builder: () => Link.Create(new string('a', 1001)),
                    expectedFieldId: nameof(Link),
                    expectedErrorCode: LinkValidationErrorCode.WrongLength),
                new ValidationErrorTestCase<LinkValidationErrorCode, Link>(
                    builder: () => Link.Create("https//not-valid-link"),
                    expectedFieldId: nameof(Link),
                    expectedErrorCode: LinkValidationErrorCode.InvalidFormat)
            }.AsReadOnly();

        [Test]
        public void DoesNotCreateLinkWhenThereIsValidationError()
        {
            ValidationErrorTestsRunner.Run(_validationErrorTestCases);
        }
    }
}