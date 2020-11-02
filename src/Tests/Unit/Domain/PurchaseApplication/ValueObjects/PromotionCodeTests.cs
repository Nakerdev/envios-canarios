using System.Collections.Generic;
using CanaryDeliveries.Domain.PurchaseApplication.ValueObjects;
using CanaryDeliveries.Tests.Domain.PurchaseApplication.Utils;
using FluentAssertions;
using NUnit.Framework;
using static LanguageExt.Prelude;

namespace CanaryDeliveries.Tests.Domain.PurchaseApplication.ValueObjects
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
        
        private readonly IReadOnlyList<ValidationErrorTestCase<PromotionCodeValidationErrorCode, PromotionCode>> _validationErrorTestCases =
            new List<ValidationErrorTestCase<PromotionCodeValidationErrorCode, PromotionCode>>
            {
                new ValidationErrorTestCase<PromotionCodeValidationErrorCode, PromotionCode>(
                    builder: () => PromotionCode.Create(None),
                    expectedFieldId: nameof(PromotionCode),
                    expectedErrorCode: PromotionCodeValidationErrorCode.Required),
                new ValidationErrorTestCase<PromotionCodeValidationErrorCode, PromotionCode>(
                    builder: () => PromotionCode.Create(new string('a', 51)),
                    expectedFieldId: nameof(PromotionCode),
                    expectedErrorCode: PromotionCodeValidationErrorCode.WrongLength),
            }.AsReadOnly();

        [Test]
        public void DoesNotCreatePromotionCodeWhenThereIsValidationError()
        {
            ValidationErrorTestsRunner.Run(_validationErrorTestCases);
        }
    }
}