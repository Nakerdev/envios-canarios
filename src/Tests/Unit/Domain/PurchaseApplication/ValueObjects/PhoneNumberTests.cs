using System.Collections.Generic;
using CanaryDeliveries.Domain.PurchaseApplication.ValueObjects;
using CanaryDeliveries.Tests.Domain.PurchaseApplication.Utils;
using FluentAssertions;
using NUnit.Framework;
using static LanguageExt.Prelude;

namespace CanaryDeliveries.Tests.Domain.PurchaseApplication.ValueObjects
{
    [TestFixture]
    public sealed class PhoneNumberTests
    {
        [Test]
        public void CreatesPhoneNumber()
        {
            var result = PhoneNumber.Create("610989898");

            result.IsSuccess.Should().BeTrue();
        }
        
        private readonly IReadOnlyList<ValidationErrorTestCase<PhoneNumberValidationErrorCode, PhoneNumber>> _validationErrorTestCases =
            new List<ValidationErrorTestCase<PhoneNumberValidationErrorCode, PhoneNumber>>
            {
                new ValidationErrorTestCase<PhoneNumberValidationErrorCode, PhoneNumber>(
                    builder: () => PhoneNumber.Create(None),
                    expectedFieldId: nameof(PhoneNumber),
                    expectedErrorCode: PhoneNumberValidationErrorCode.Required),
                new ValidationErrorTestCase<PhoneNumberValidationErrorCode, PhoneNumber>(
                    builder: () => PhoneNumber.Create("not-a-number"),
                    expectedFieldId: nameof(PhoneNumber),
                    expectedErrorCode: PhoneNumberValidationErrorCode.InvalidFormat),
                new ValidationErrorTestCase<PhoneNumberValidationErrorCode, PhoneNumber>(
                    builder: () => PhoneNumber.Create(new string('1', 16)),
                    expectedFieldId: nameof(PhoneNumber),
                    expectedErrorCode: PhoneNumberValidationErrorCode.WrongLenght)
            }.AsReadOnly();

        [Test]
        public void DoesNotCreatePhoneNumberWhenThereIsValidationError()
        {
            ValidationErrorTestsRunner.Run(_validationErrorTestCases);
        }
    }
}