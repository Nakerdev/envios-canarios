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
        public void CreatesLink()
        {
            const string link = "https://adidas.com/product/0";
            
            var result = Link.Create(link);

            result.IsSuccess.Should().BeTrue();
        }
        
        private readonly IReadOnlyList<ValidationErrorTestCase<GenericValidationErrorCode, Link>> _validationErrorTestCases =
            new List<ValidationErrorTestCase<GenericValidationErrorCode, Link>>
            {
                new ValidationErrorTestCase<GenericValidationErrorCode, Link>(
                    builder: () => Link.Create(None),
                    expectedFieldId: nameof(Link),
                    expectedErrorCode: GenericValidationErrorCode.Required),
                new ValidationErrorTestCase<GenericValidationErrorCode, Link>(
                    builder: () => Link.Create(new string('a', 1001)),
                    expectedFieldId: nameof(Link),
                    expectedErrorCode: GenericValidationErrorCode.WrongLength),
                new ValidationErrorTestCase<GenericValidationErrorCode, Link>(
                    builder: () => Link.Create("https//not-valid-link"),
                    expectedFieldId: nameof(Link),
                    expectedErrorCode: GenericValidationErrorCode.InvalidFormat)
            }.AsReadOnly();

        [Test]
        public void DoesNotCreateLinkWhenThereIsValidationError()
        {
            ValidationErrorTestsRunner.Run(_validationErrorTestCases);
        }
    }
}