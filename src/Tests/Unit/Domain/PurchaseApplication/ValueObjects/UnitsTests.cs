using System.Collections.Generic;
using CanaryDeliveries.Domain.PurchaseApplication.ValueObjects;
using CanaryDeliveries.Tests.Domain.PurchaseApplication.Utils;
using FluentAssertions;
using NUnit.Framework;
using static LanguageExt.Prelude;

namespace CanaryDeliveries.Tests.Domain.PurchaseApplication.ValueObjects
{
    [TestFixture]
    public sealed class UnitsTests
    {
        [Test]
        public void CreatesUnits()
        {
            var result = Units.Create("1");

            result.IsSuccess.Should().BeTrue();
        }
        
        private readonly IReadOnlyList<ValidationErrorTestCase<UnitsValidationErrorCode, Units>> _validationErrorTestCases =
            new List<ValidationErrorTestCase<UnitsValidationErrorCode, Units>>
            {
                new ValidationErrorTestCase<UnitsValidationErrorCode, Units>(
                    builder: () => Units.Create(None),
                    expectedFieldId: nameof(Units),
                    expectedErrorCode: UnitsValidationErrorCode.Required),
                new ValidationErrorTestCase<UnitsValidationErrorCode, Units>(
                    builder: () => Units.Create("not-a-number"),
                    expectedFieldId: nameof(Units),
                    expectedErrorCode: UnitsValidationErrorCode.InvalidFormat),
                new ValidationErrorTestCase<UnitsValidationErrorCode, Units>(
                    builder: () => Units.Create("0"),
                    expectedFieldId: nameof(Units),
                    expectedErrorCode: UnitsValidationErrorCode.InvalidValue)
            }.AsReadOnly();

        [Test]
        public void DoesNotCreateUnitsWhenThereIsValidationError()
        {
            ValidationErrorTestsRunner.Run(_validationErrorTestCases);
        }
    }
}