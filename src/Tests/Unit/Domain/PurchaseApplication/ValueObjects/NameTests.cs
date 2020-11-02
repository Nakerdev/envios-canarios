using System.Collections.Generic;
using CanaryDeliveries.Domain.PurchaseApplication.ValueObjects;
using CanaryDeliveries.Tests.Domain.PurchaseApplication.Utils;
using FluentAssertions;
using NUnit.Framework;
using static LanguageExt.Prelude;

namespace CanaryDeliveries.Tests.Domain.PurchaseApplication.ValueObjects
{
    [TestFixture]
    public sealed class NameTests
    {
        [Test]
        public void CreatesName()
        {
            var result = Name.Create("Alfredo");

            result.IsSuccess.Should().BeTrue();
        }
        
        private readonly IReadOnlyList<ValidationErrorTestCase<NameValidationErrorCode, Name>> _validationErrorTestCases =
            new List<ValidationErrorTestCase<NameValidationErrorCode, Name>>
            {
                new ValidationErrorTestCase<NameValidationErrorCode, Name>(
                    builder: () => Name.Create(None),
                    expectedFieldId: nameof(Name),
                    expectedErrorCode: NameValidationErrorCode.Required),
                new ValidationErrorTestCase<NameValidationErrorCode, Name>(
                    builder: () => Name.Create(new string('a', 256)),
                    expectedFieldId: nameof(Name),
                    expectedErrorCode: NameValidationErrorCode.WrongLength),
            }.AsReadOnly();

        [Test]
        public void DoesNotCreateNameWhenThereIsValidationError()
        {
            ValidationErrorTestsRunner.Run(_validationErrorTestCases);
        }
    }
}