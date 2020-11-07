using System.Collections.Generic;
using CanaryDeliveries.PurchaseApplication.Domain.ValueObjects;
using CanaryDeliveries.Tests.PurchaseApplication.Unit.Domain.Utils;
using FluentAssertions;
using NUnit.Framework;
using static LanguageExt.Prelude;

namespace CanaryDeliveries.Tests.PurchaseApplication.Unit.Domain.ValueObjects
{
    [TestFixture]
    public sealed class PromotionCodeTests
    {
        [Test]
        public void CreatesPromotionCode()
        {
            var result = PromotionCode.Create("ADDIDAS-123");

            result.IsSuccess.Should().BeTrue();
        }
        
        private readonly IReadOnlyList<ValidationErrorTestCase<GenericValidationErrorCode, PromotionCode>> _validationErrorTestCases =
            new List<ValidationErrorTestCase<GenericValidationErrorCode, PromotionCode>>
            {
                new ValidationErrorTestCase<GenericValidationErrorCode, PromotionCode>(
                    builder: () => PromotionCode.Create(None),
                    expectedFieldId: nameof(PromotionCode),
                    expectedErrorCode: GenericValidationErrorCode.Required),
                new ValidationErrorTestCase<GenericValidationErrorCode, PromotionCode>(
                    builder: () => PromotionCode.Create(new string('a', 51)),
                    expectedFieldId: nameof(PromotionCode),
                    expectedErrorCode: GenericValidationErrorCode.WrongLength),
            }.AsReadOnly();

        [Test]
        public void DoesNotCreatePromotionCodeWhenThereIsValidationError()
        {
            ValidationErrorTestsRunner.Run(_validationErrorTestCases);
        }
    }
}