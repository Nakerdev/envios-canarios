using System;
using System.Collections.Generic;
using CanaryDeliveries.Domain.PurchaseApplication.Create;
using CanaryDeliveries.Domain.PurchaseApplication.ValueObjects;
using FluentAssertions;
using LanguageExt;
using NUnit.Framework;
using static LanguageExt.Prelude;

namespace CanaryDeliveries.Tests.Domain.PurchaseApplication.ValueObjects
{
    [TestFixture]
    public sealed class LinkTests
    {
        [Test]
        public void CreatesALink()
        {
            const string link = "https://adidas.com/product/0";
            
            var result = Link.Create(link);

            result.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void DoesNotCreateALinkWhenValueIsEmpty()
        {
            var result = Link.Create(None);

            result.IsFail.Should().BeTrue();
            result.IfFail(validationErrors =>
            {
                validationErrors.Count.Should().Be(1);
                validationErrors.First().FieldId.Should().Be(nameof(Link));
                validationErrors.First().ErrorCode.Should().Be(LinkValidationErrorCode.Required);
            });
        }
        
        [Test]
        public void DoesNotCreateALinkWhenValueIsTooLong()
        {
            var link = new string('a', 1001);
            
            var result = Link.Create(link);

            result.IsFail.Should().BeTrue();
            result.IfFail(validationErrors =>
            {
                validationErrors.Count.Should().Be(1);
                validationErrors.First().FieldId.Should().Be(nameof(Link));
                validationErrors.First().ErrorCode.Should().Be(LinkValidationErrorCode.WrongLength);
            });
        }
        
        [Test]
        public void DoesNotCreateALinkWhenValueIsNotAValidLinkFormat()
        {
            const string link = "https//not-valid-link";
            
            var result = Link.Create(link);

            result.IsFail.Should().BeTrue();
            result.IfFail(validationErrors =>
            {
                validationErrors.Count.Should().Be(1);
                validationErrors.First().FieldId.Should().Be(nameof(Link));
                validationErrors.First().ErrorCode.Should().Be(LinkValidationErrorCode.InvalidFormat);
            });
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
        
        private readonly IReadOnlyList<ValidationErrorTestCase<LinkValidationErrorCode, Link>> _validationErrorTestCases =
            new List<ValidationErrorTestCase<LinkValidationErrorCode, Link>>
            {
                new ValidationErrorTestCase<LinkValidationErrorCode, Link>(
                    builder: () => Link.Create(None),
                    expectedFieldId: nameof(Link),
                    expectedErrorCode: LinkValidationErrorCode.Required),
                new ValidationErrorTestCase<LinkValidationErrorCode, Link>(
                    builder: () => Link.Create(new string('a', 1001)),
                    expectedFieldId: nameof(Link),
                    expectedErrorCode: LinkValidationErrorCode.WrongLength),
                new ValidationErrorTestCase<LinkValidationErrorCode, Link>(
                    builder: () => Link.Create("https//not-valid-link"),
                    expectedFieldId: nameof(Link),
                    expectedErrorCode: LinkValidationErrorCode.InvalidFormat)
            }.AsReadOnly();

        [Test]
        public void DoesNotCreateALinkWhenThereIsValidationError()
        {
            foreach (var validationErrorTestCase in _validationErrorTestCases)
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
}