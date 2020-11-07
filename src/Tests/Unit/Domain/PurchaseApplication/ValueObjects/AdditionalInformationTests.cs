using System.Collections.Generic;
using CanaryDeliveries.PurchaseApplication.Domain.ValueObjects;
using CanaryDeliveries.Tests.Domain.PurchaseApplication.Utils;
using FluentAssertions;
using NUnit.Framework;
using static LanguageExt.Prelude;

namespace CanaryDeliveries.Tests.Domain.PurchaseApplication.ValueObjects
{
    [TestFixture]
    public sealed class AdditionalInformationTests
    {
        [Test]
        public void CreatesAdditionalInformation()
        {
            var result = AdditionalInformation.Create("Additional information");

            result.IsSuccess.Should().BeTrue();
        }
        
        private readonly IReadOnlyList<ValidationErrorTestCase<GenericValidationErrorCode, AdditionalInformation>> _validationErrorTestCases =
            new List<ValidationErrorTestCase<GenericValidationErrorCode, AdditionalInformation>>
            {
                new ValidationErrorTestCase<GenericValidationErrorCode, AdditionalInformation>(
                    builder: () => AdditionalInformation.Create(None),
                    expectedFieldId: nameof(AdditionalInformation),
                    expectedErrorCode: GenericValidationErrorCode.Required),
                new ValidationErrorTestCase<GenericValidationErrorCode, AdditionalInformation>(
                    builder: () => AdditionalInformation.Create(new string('a', 1001)),
                    expectedFieldId: nameof(AdditionalInformation),
                    expectedErrorCode: GenericValidationErrorCode.WrongLength),
            }.AsReadOnly();

        [Test]
        public void DoesNotCreateAdditionalInformationWhenThereIsValidationError()
        {
            ValidationErrorTestsRunner.Run(_validationErrorTestCases);
        }
    }
}