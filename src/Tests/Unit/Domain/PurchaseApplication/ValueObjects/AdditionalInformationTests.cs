using System.Collections.Generic;
using CanaryDeliveries.Domain.PurchaseApplication.ValueObjects;
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
        public void CreatesALink()
        {
            var result = AdditionalInformation.Create("Additional information");

            result.IsSuccess.Should().BeTrue();
        }
        
        private readonly IReadOnlyList<ValidationErrorTestCase<AdditionalInformationValidationErrorCode, AdditionalInformation>> _validationErrorTestCases =
            new List<ValidationErrorTestCase<AdditionalInformationValidationErrorCode, AdditionalInformation>>
            {
                new ValidationErrorTestCase<AdditionalInformationValidationErrorCode, AdditionalInformation>(
                    builder: () => AdditionalInformation.Create(None),
                    expectedFieldId: nameof(AdditionalInformation),
                    expectedErrorCode: AdditionalInformationValidationErrorCode.Required),
                new ValidationErrorTestCase<AdditionalInformationValidationErrorCode, AdditionalInformation>(
                    builder: () => AdditionalInformation.Create(new string('a', 1001)),
                    expectedFieldId: nameof(AdditionalInformation),
                    expectedErrorCode: AdditionalInformationValidationErrorCode.WrongLength),
            }.AsReadOnly();

        [Test]
        public void DoesNotCreateAdditionalInformationWhenThereIsValidationError()
        {
            ValidationErrorTestsRunner.Run(_validationErrorTestCases);
        }
    }
}