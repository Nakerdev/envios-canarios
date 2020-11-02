using System;
using CanaryDeliveries.Domain.PurchaseApplication.Create;
using LanguageExt;

namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class Link : Record<Link>
    {
        private string value;
        
        public static Validation<ValidationError<LinkValidationErrorCode>, Link> Create(
            Option<string> value)
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
                    .ToValidation(new ValidationError<LinkValidationErrorCode>(
                        fieldId: nameof(Link),
                        errorCode: LinkValidationErrorCode.Required));
            }
            
            Validation<ValidationError<LinkValidationErrorCode>, Link> ValidateLenght(Link link)
            {
                const int maxAllowedLenght = 1000;
                if (link.value.Length > maxAllowedLenght)
                {
                    return new ValidationError<LinkValidationErrorCode>(
                        fieldId: nameof(Link),
                        errorCode: LinkValidationErrorCode.WrongLength);
                }
                return link;
            }
            
            Validation<ValidationError<LinkValidationErrorCode>, Link> ValidateFormat(Link link)
            {
                if (!Uri.IsWellFormedUriString(link.value, UriKind.Absolute))
                {
                    return new ValidationError<LinkValidationErrorCode>(
                        fieldId: nameof(Link),
                        errorCode: LinkValidationErrorCode.InvalidFormat);
                }
                return link;
            }
        }

        public Link(string value)
        {
            this.value = value;
        }
    }

    public enum LinkValidationErrorCode
    {
        Required,
        InvalidFormat,
        WrongLength
    }
}