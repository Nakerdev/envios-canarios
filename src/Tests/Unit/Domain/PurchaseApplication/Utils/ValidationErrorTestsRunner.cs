using System;
using System.Collections.Generic;
using CanaryDeliveries.Domain.PurchaseApplication.Create;
using FluentAssertions;
using LanguageExt;

namespace CanaryDeliveries.Tests.Domain.PurchaseApplication.Utils
{
    public static class ValidationErrorTestsRunner
    {
        public static void Run<IValidationErrorCode, IType>(
            IEnumerable<ValidationErrorTestCase<IValidationErrorCode, IType>> validationErrorTestCases)
        {
            foreach (var validationErrorTestCase in validationErrorTestCases)
            {
                var result = validationErrorTestCase.Builder();
                         
                 result.IsFail.Should().BeTrue();
                 result.IfFail(validationErrors =>
                 {
                     validationErrors.Count.Should().Be(1);
                     validationErrors.First().FieldId.Should().Be(validationErrorTestCase.ExpectedFieldId);
                     validationErrors.First().ErrorCode.Should().Be(validationErrorTestCase.ExpectedErrorCode);
                 });                   
            }
        }
    }
    
    public class ValidationErrorTestCase<IValidationErrorCode, IType>
    {
        public Func<Validation<ValidationError<IValidationErrorCode>, IType>> Builder { get; }
        public string ExpectedFieldId { get; }
        public IValidationErrorCode ExpectedErrorCode { get; }

        public ValidationErrorTestCase(
            Func<Validation<ValidationError<IValidationErrorCode>, IType>> builder, 
            string expectedFieldId, 
            IValidationErrorCode expectedErrorCode)
        {
            Builder = builder;
            ExpectedFieldId = expectedFieldId;
            ExpectedErrorCode = expectedErrorCode;
        }
    }
}