using System.Collections.Generic;
using CanaryDeliveries.PurchaseApplication.Domain.ValueObjects;
using CanaryDeliveries.Tests.PurchaseApplication.Unit.DomainTests.Utils;
using FluentAssertions;
using NUnit.Framework;
using static LanguageExt.Prelude;

namespace CanaryDeliveries.Tests.PurchaseApplication.Unit.DomainTests.ValueObjects
{
    [TestFixture]
    public sealed class RejectTests
    {
        [Test]
        public void CreatesReject()
        {
            var result = Rejection.Create(
                optionalDateTime: "2020-10-10T12:30:00Z",
                optionalReason: "Razon del rechazo");

            result.IsSuccess.Should().BeTrue();
        }
        
        private readonly IReadOnlyList<ValidationErrorTestCase<GenericValidationErrorCode, Rejection>> _validationErrorTestCases =
            new List<ValidationErrorTestCase<GenericValidationErrorCode, Rejection>>
            {
                new ValidationErrorTestCase<GenericValidationErrorCode, Rejection>(
                    builder: () => Rejection.Create(optionalDateTime: None, optionalReason: "Razon del rechazo"),
                    expectedFieldId: "RejectionDateTime",
                    expectedErrorCode: GenericValidationErrorCode.Required),
               new ValidationErrorTestCase<GenericValidationErrorCode, Rejection>(
                   builder: () => Rejection.Create(optionalDateTime: "not-valid-date-time", optionalReason: "Razon del rechazo"),
                   expectedFieldId: "RejectionDateTime",
                   expectedErrorCode: GenericValidationErrorCode.InvalidFormat), 
                new ValidationErrorTestCase<GenericValidationErrorCode, Rejection>(
                    builder: () => Rejection.Create(optionalDateTime: "2020-10-10", optionalReason: None),
                    expectedFieldId: "RejectionReason",
                    expectedErrorCode: GenericValidationErrorCode.Required),
                new ValidationErrorTestCase<GenericValidationErrorCode, Rejection>(
                    builder: () => Rejection.Create(optionalDateTime: "2020-10-10", optionalReason: new string('a', 1001)),
                    expectedFieldId: "RejectionReason",
                    expectedErrorCode: GenericValidationErrorCode.WrongLength),
            }.AsReadOnly();

        [Test]
        public void DoesNotCreateRejectWhenThereIsValidationError()
        {
            ValidationErrorTestsRunner.Run(_validationErrorTestCases);
        }
    }
}