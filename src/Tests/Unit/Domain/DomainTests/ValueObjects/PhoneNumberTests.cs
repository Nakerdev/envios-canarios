using System.Collections.Generic;
using CanaryDeliveries.PurchaseApplication.Domain.ValueObjects;
using CanaryDeliveries.Tests.PurchaseApplication.Unit.DomainTests.Utils;
using FluentAssertions;
using NUnit.Framework;
using static LanguageExt.Prelude;

namespace CanaryDeliveries.Tests.PurchaseApplication.Unit.DomainTests.ValueObjects
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
        
        private readonly IReadOnlyList<ValidationErrorTestCase<GenericValidationErrorCode, PhoneNumber>> _validationErrorTestCases =
            new List<ValidationErrorTestCase<GenericValidationErrorCode, PhoneNumber>>
            {
                new ValidationErrorTestCase<GenericValidationErrorCode, PhoneNumber>(
                    builder: () => PhoneNumber.Create(None),
                    expectedFieldId: nameof(PhoneNumber),
                    expectedErrorCode: GenericValidationErrorCode.Required),
                new ValidationErrorTestCase<GenericValidationErrorCode, PhoneNumber>(
                    builder: () => PhoneNumber.Create("not-a-number"),
                    expectedFieldId: nameof(PhoneNumber),
                    expectedErrorCode: GenericValidationErrorCode.InvalidFormat),
                new ValidationErrorTestCase<GenericValidationErrorCode, PhoneNumber>(
                    builder: () => PhoneNumber.Create(new string('1', 16)),
                    expectedFieldId: nameof(PhoneNumber),
                    expectedErrorCode: GenericValidationErrorCode.WrongLength)
            }.AsReadOnly();

        [Test]
        public void DoesNotCreatePhoneNumberWhenThereIsValidationError()
        {
            ValidationErrorTestsRunner.Run(_validationErrorTestCases);
        }
    }
}