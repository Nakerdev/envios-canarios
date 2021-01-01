using System;
using LanguageExt;
using static LanguageExt.Prelude;

namespace CanaryDeliveries.PurchaseApplication.Domain.ValueObjects
{
    public sealed class Id : Record<Id>
    {
        public PersistenceState State => new PersistenceState(value.ToString());
        
        private readonly Guid value;
        
        public static Validation<ValidationError<GenericValidationErrorCode>, Id> Create(
            Option<string> value)
        {
            return
                from id in ValidateRequire()
                from _1 in ValidateFormat(id)
                select BuildId(id);

            Validation<ValidationError<GenericValidationErrorCode>, string> ValidateRequire()
            {
                return value
                    .ToValidation(CreateValidationError(GenericValidationErrorCode.Required));
            }
            
            Validation<ValidationError<GenericValidationErrorCode>, Unit> ValidateFormat(string id)
            {
                if (!Guid.TryParse(id, out _))
                {
                    return CreateValidationError(GenericValidationErrorCode.InvalidFormat);
                };
                return unit;
            }
            
            static Id BuildId(string id)
            {
                return new Id(new Guid(id));
            }

            ValidationError<GenericValidationErrorCode> CreateValidationError(
                GenericValidationErrorCode errorCode)
            {
                return new ValidationError<GenericValidationErrorCode>(
                    fieldId: nameof(Id), 
                    errorCode: errorCode);
            }
        }

        public static Id Create()
        {
            return new Id(Guid.NewGuid());
        }

        public Id(PersistenceState persistenceState)
        {
            value = new Guid(persistenceState.Value);
        }

        private Id(Guid value)
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