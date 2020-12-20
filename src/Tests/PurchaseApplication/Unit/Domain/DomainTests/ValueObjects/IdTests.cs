using System.Collections.Generic;
using CanaryDeliveries.PurchaseApplication.Domain.ValueObjects;
using CanaryDeliveries.Tests.PurchaseApplication.Unit.DomainTests.Utils;
using FluentAssertions;
using NUnit.Framework;
using static LanguageExt.Prelude;

namespace CanaryDeliveries.Tests.PurchaseApplication.Unit.DomainTests.ValueObjects
{
    [TestFixture]
    public sealed class IdTests
    {
        [Test]
        public void CreatesId()
        {
            var result = Id.Create("b5cd78a5-2e26-498a-a399-2c5cb2bf0f54");

            result.IsSuccess.Should().BeTrue();
        }
        
        private readonly IReadOnlyList<ValidationErrorTestCase<GenericValidationErrorCode, Id>> _validationErrorTestCases =
            new List<ValidationErrorTestCase<GenericValidationErrorCode, Id>>
            {
                new ValidationErrorTestCase<GenericValidationErrorCode, Id>(
                    builder: () => Id.Create(None),
                    expectedFieldId: nameof(Id),
                    expectedErrorCode: GenericValidationErrorCode.Required),
                new ValidationErrorTestCase<GenericValidationErrorCode, Id>(
                    builder: () => Id.Create("not-valid-format"),
                    expectedFieldId: nameof(Id),
                    expectedErrorCode: GenericValidationErrorCode.InvalidFormat)
            }.AsReadOnly();

        [Test]
        public void DoesNotCreateIdWhenThereIsValidationError()
        {
            ValidationErrorTestsRunner.Run(_validationErrorTestCases);
        }
    }
}