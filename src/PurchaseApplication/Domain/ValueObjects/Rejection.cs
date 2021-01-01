using System;
using LanguageExt;
using static LanguageExt.Prelude;

namespace CanaryDeliveries.PurchaseApplication.Domain.ValueObjects
{
    public sealed class Rejection : Record<Rejection>
    {
        public PersistenceState State => new PersistenceState(
            dateTime: dateTime, 
            reason: reason.State);
        
        private readonly DateTime dateTime;
        private readonly RejectionReason reason;

        public Rejection(PersistenceState persistenceState)
        {
            dateTime = persistenceState.DateTime;
            reason = new RejectionReason(persistenceState.Reason);;
        }

        public Rejection(DateTime dateTime, RejectionReason reason)
        {
            this.dateTime = dateTime;
            this.reason = reason;
        }

        public sealed class PersistenceState
        {
            public DateTime DateTime { get; }
            public RejectionReason.PersistenceState Reason { get; }

            public PersistenceState(DateTime dateTime, RejectionReason.PersistenceState reason)
            {
                DateTime = dateTime;
                Reason = reason;
            }
        }
    }
    
    public sealed class RejectionReason : Record<RejectionReason>
    {
        public PersistenceState State => new PersistenceState(value: value);
        
        private readonly string value;

        public static Validation<ValidationError<GenericValidationErrorCode>, RejectionReason> Create(
            Option<string> value)
        {
            return
                from reason in ValidateRequire()
                from _1 in ValidateLenght(reason)
                select BuildRejectionReason(reason);

            Validation<ValidationError<GenericValidationErrorCode>, string> ValidateRequire()
            {
                return value
                    .ToValidation(CreateValidationError(GenericValidationErrorCode.Required));
            }
            
            Validation<ValidationError<GenericValidationErrorCode>, Unit> ValidateLenght(string reason)
            {
                const int maxAllowedLenght = 1000;
                if (reason.Length > maxAllowedLenght)
                {
                    return CreateValidationError(GenericValidationErrorCode.WrongLength);
                }
                return unit;
            }
            
            static RejectionReason BuildRejectionReason(string reason)
            {
                return new RejectionReason(reason);
            }

            ValidationError<GenericValidationErrorCode> CreateValidationError(
                GenericValidationErrorCode errorCode)
            {
                return new ValidationError<GenericValidationErrorCode>(
                    fieldId: nameof(RejectionReason), 
                    errorCode: errorCode);
            }
        }

        public RejectionReason(PersistenceState persistenceState)
        {
            value = persistenceState.Value;
        }

        private RejectionReason(string value)
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