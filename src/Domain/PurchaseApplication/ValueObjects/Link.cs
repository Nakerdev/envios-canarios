using System;
using CanaryDeliveries.Domain.PurchaseApplication.Create;
using LanguageExt;

namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class Link : Record<Link>
    {
        private readonly string Value;
        
        public static Validation<ValidationError<LinkValidationErrorCode>, Link> Create(Option<string> value)
        {
            return
                from link in ValidateRequire(value)
                from _1 in ValidateLenght(link)
                from _2 in ValidateFormat(link)
                select link;

            Validation<ValidationError<LinkValidationErrorCode>, Link> ValidateRequire(Option<string> val)
            {
                return val
                    .Map(v => new Link(v))
                    .ToValidation(CreateValidationError(LinkValidationErrorCode.Required));
            }
            
            Validation<ValidationError<LinkValidationErrorCode>, Link> ValidateLenght(Link link)
            {
                const int maxAllowedLenght = 1000;
                if (link.Value.Length > maxAllowedLenght)
                {
                    return CreateValidationError(LinkValidationErrorCode.WrongLength);
                }
                return link;
            }
            
            Validation<ValidationError<LinkValidationErrorCode>, Link> ValidateFormat(Link link)
            {
                if (!Uri.IsWellFormedUriString(link.Value, UriKind.Absolute))
                {
                    return CreateValidationError(LinkValidationErrorCode.InvalidFormat);
                }
                return link;
            }

            ValidationError<LinkValidationErrorCode> CreateValidationError(LinkValidationErrorCode errorCode)
            {
                return new ValidationError<LinkValidationErrorCode>(fieldId: nameof(Link), errorCode: errorCode);
            }
        }

        private Link(string value)
        {
            Value = value;
        }
    }

    public enum LinkValidationErrorCode
    {
        Required,
        InvalidFormat,
        WrongLength
    }
}