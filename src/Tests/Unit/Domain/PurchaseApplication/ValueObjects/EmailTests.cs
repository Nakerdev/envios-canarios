using System.Collections.Generic;
using CanaryDeliveries.Domain.PurchaseApplication.ValueObjects;
using CanaryDeliveries.Tests.Domain.PurchaseApplication.Utils;
using FluentAssertions;
using NUnit.Framework;
using static LanguageExt.Prelude;

namespace CanaryDeliveries.Tests.Domain.PurchaseApplication.ValueObjects
{
    [TestFixture]
    public sealed class EmailTests
    {
        [Test]
        public void CreatesEmail()
        {
            var result = Email.Create("user@email.com");

            result.IsSuccess.Should().BeTrue();
        }
        
        private readonly IReadOnlyList<ValidationErrorTestCase<EmailValidationErrorCode, Email>> _validationErrorTestCases =
            new List<ValidationErrorTestCase<EmailValidationErrorCode, Email>>
            {
                new ValidationErrorTestCase<EmailValidationErrorCode, Email>(
                    builder: () => Email.Create(None),
                    expectedFieldId: nameof(Email),
                    expectedErrorCode: EmailValidationErrorCode.Required),
                new ValidationErrorTestCase<EmailValidationErrorCode, Email>(
                    builder: () => Email.Create("not-valid-email"),
                    expectedFieldId: nameof(Email),
                    expectedErrorCode: EmailValidationErrorCode.InvalidFormat),
                new ValidationErrorTestCase<EmailValidationErrorCode, Email>(
                    builder: () => Email.Create(new string('a', 256)),
                    expectedFieldId: nameof(Email),
                    expectedErrorCode: EmailValidationErrorCode.WrongLength),
            }.AsReadOnly();

        [Test]
        public void DoesNotCreateEmailWhenThereIsValidationError()
        {
            ValidationErrorTestsRunner.Run(_validationErrorTestCases);
        }
    }
}