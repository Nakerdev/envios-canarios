using System.Collections.Generic;
using CanaryDeliveries.PurchaseApplication.Domain.ValueObjects;
using CanaryDeliveries.Tests.PurchaseApplication.Unit.DomainTests.Utils;
using FluentAssertions;
using NUnit.Framework;
using static LanguageExt.Prelude;

namespace CanaryDeliveries.Tests.PurchaseApplication.Unit.DomainTests.ValueObjects
{
    [TestFixture]
    public sealed class RejectionReasonTests
    {
        [Test]
        public void CreatesReject()
        {
            var result = RejectionReason.Create(optionalReason: "Razon del rechazo");

            result.IsSuccess.Should().BeTrue();
        }
        
        private readonly IReadOnlyList<ValidationErrorTestCase<GenericValidationErrorCode, RejectionReason>> _validationErrorTestCases =
            new List<ValidationErrorTestCase<GenericValidationErrorCode, RejectionReason>>
            {
                new ValidationErrorTestCase<GenericValidationErrorCode, RejectionReason>(
                    builder: () => RejectionReason.Create(optionalReason: None),
                    expectedFieldId: nameof(RejectionReason),
                    expectedErrorCode: GenericValidationErrorCode.Required),
                new ValidationErrorTestCase<GenericValidationErrorCode, RejectionReason>(
                    builder: () => RejectionReason.Create(optionalReason: new string('a', 1001)),
                    expectedFieldId: nameof(RejectionReason),
                    expectedErrorCode: GenericValidationErrorCode.WrongLength),
            }.AsReadOnly();

        [Test]
        public void DoesNotCreateRejectWhenThereIsValidationError()
        {
            ValidationErrorTestsRunner.Run(_validationErrorTestCases);
        }
    }
}