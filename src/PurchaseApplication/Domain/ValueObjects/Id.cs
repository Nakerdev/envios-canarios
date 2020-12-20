using System;
using LanguageExt;

namespace CanaryDeliveries.PurchaseApplication.Domain.ValueObjects
{
    public sealed class Id : Record<Id>
    {
        public PersistenceState State => new PersistenceState(value.ToString());
        
        private readonly Guid value;
        
        public static Validation<ValidationError<GenericValidationErrorCode>, Id> Create(
            Option<string> optionalId)
        {
            return
                from flatId in ValidateRequire()
                from formattedId in ValidateFormat(flatId)
                from id in BuildId(formattedId)
                select id;

            Validation<ValidationError<GenericValidationErrorCode>, string> ValidateRequire()
            {
                return optionalId
                    .ToValidation(CreateValidationError(GenericValidationErrorCode.Required));
            }
            
            Validation<ValidationError<GenericValidationErrorCode>, Guid> ValidateFormat(string id)
            {
                try
                {
                    return new Guid(id);
                }
                catch (Exception e)
                {
                    return CreateValidationError(GenericValidationErrorCode.InvalidFormat);
                }
            }
            
            Validation<ValidationError<GenericValidationErrorCode>, Id> BuildId(Guid id)
            {
                return new Id(id);
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