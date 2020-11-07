using System.Collections.Generic;
using CanaryDeliveries.PurchaseApplication.Domain.ValueObjects;
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
        
        private readonly IReadOnlyList<ValidationErrorTestCase<GenericValidationErrorCode, Units>> _validationErrorTestCases =
            new List<ValidationErrorTestCase<GenericValidationErrorCode, Units>>
            {
                new ValidationErrorTestCase<GenericValidationErrorCode, Units>(
                    builder: () => Units.Create(None),
                    expectedFieldId: nameof(Units),
                    expectedErrorCode: GenericValidationErrorCode.Required),
                new ValidationErrorTestCase<GenericValidationErrorCode, Units>(
                    builder: () => Units.Create("not-a-number"),
                    expectedFieldId: nameof(Units),
                    expectedErrorCode: GenericValidationErrorCode.InvalidFormat),
                new ValidationErrorTestCase<GenericValidationErrorCode, Units>(
                    builder: () => Units.Create("0"),
                    expectedFieldId: nameof(Units),
                    expectedErrorCode: GenericValidationErrorCode.InvalidValue)
            }.AsReadOnly();

        [Test]
        public void DoesNotCreateUnitsWhenThereIsValidationError()
        {
            ValidationErrorTestsRunner.Run(_validationErrorTestCases);
        }
    }
}