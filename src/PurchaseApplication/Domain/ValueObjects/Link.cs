using System;
using System.Runtime;
using LanguageExt;

namespace CanaryDeliveries.PurchaseApplication.Domain.ValueObjects
{
    public sealed class Link : Record<Link>
    {
        public PersistenceState State => new PersistenceState(value);
        
        private readonly string value;
        
        public static Validation<ValidationError<GenericValidationErrorCode>, Link> Create(Option<string> value)
        {
            return
                from link in ValidateRequire(value)
                from _1 in ValidateLenght(link)
                from _2 in ValidateFormat(link)
                select link;

            Validation<ValidationError<GenericValidationErrorCode>, Link> ValidateRequire(Option<string> val)
            {
                return val
                    .Map(v => new Link(v))
                    .ToValidation(CreateValidationError(GenericValidationErrorCode.Required));
            }
            
            Validation<ValidationError<GenericValidationErrorCode>, Link> ValidateLenght(Link link)
            {
                const int maxAllowedLenght = 1000;
                if (link.value.Length > maxAllowedLenght)
                {
                    return CreateValidationError(GenericValidationErrorCode.WrongLength);
                }
                return link;
            }
            
            Validation<ValidationError<GenericValidationErrorCode>, Link> ValidateFormat(Link link)
            {
                if (!Uri.IsWellFormedUriString(link.value, UriKind.Absolute))
                {
                    return CreateValidationError(GenericValidationErrorCode.InvalidFormat);
                }
                return link;
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