using System;
using System.Runtime;
using LanguageExt;
using static LanguageExt.Prelude;

namespace CanaryDeliveries.PurchaseApplication.Domain.ValueObjects
{
    public sealed class Link : Record<Link>
    {
        public PersistenceState State => new PersistenceState(value);
        
        private readonly string value;
        
        public static Validation<ValidationError<GenericValidationErrorCode>, Link> Create(Option<string> value)
        {
            return
                from link in ValidateRequire()
                from _1 in ValidateLenght(link)
                from _2 in ValidateFormat(link)
                select BuildLink(link);

            Validation<ValidationError<GenericValidationErrorCode>, string> ValidateRequire()
            {
                return value
                    .ToValidation(CreateValidationError(GenericValidationErrorCode.Required));
            }
            
            Validation<ValidationError<GenericValidationErrorCode>, Unit> ValidateLenght(string link)
            {
                const int maxAllowedLenght = 1000;
                if (link.Length > maxAllowedLenght)
                {
                    return CreateValidationError(GenericValidationErrorCode.WrongLength);
                }
                return unit;
            }
            
            Validation<ValidationError<GenericValidationErrorCode>, Unit> ValidateFormat(string link)
            {
                if (!Uri.IsWellFormedUriString(link, UriKind.Absolute))
                {
                    return CreateValidationError(GenericValidationErrorCode.InvalidFormat);
                }
                return unit;
            }
            
            static Link BuildLink(string link)
            {
                return new Link(link);
            }

            ValidationError<GenericValidationErrorCode> CreateValidationError(GenericValidationErrorCode errorCode)
            {
                return new ValidationError<GenericValidationErrorCode>(fieldId: nameof(Link), errorCode: errorCode);
            }
        }
        
        public Link(PersistenceState persistenceState)
        {
            value = persistenceState.Value;
        }

        private Link(string value)
        {
            this.value = value;
        }

        public sealed class PersistenceState
        {
            public string Value { get; }

            public PersistenceState(string value)
            {
                Value = value;
            }
        }
    }
}