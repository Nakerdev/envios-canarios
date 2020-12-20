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
            Option<string> optionalReason)
        {
            return
                from reason in ValidateRequire(optionalReason)
                from _1 in ValidateLenght(reason)
                from rejectionReason in BuildRejectionReason(reason)
                select rejectionReason ;

            Validation<ValidationError<GenericValidationErrorCode>, string> ValidateRequire(
                Option<string> value)
            {
                return value
                    .ToValidation(CreateValidationError(
                        fieldId: nameof(RejectionReason),
                        errorCode: GenericValidationErrorCode.Required));
            }
            
            Validation<ValidationError<GenericValidationErrorCode>, Unit> ValidateLenght(
                string reason)
            {
                const int maxAllowedLenght = 1000;
                if (reason.Length > maxAllowedLenght)
                {
                    return CreateValidationError(
                        fieldId: nameof(RejectionReason),
                        errorCode: GenericValidationErrorCode.WrongLength);
                }
                return unit;
            }
            
            Validation<ValidationError<GenericValidationErrorCode>, RejectionReason> BuildRejectionReason(
                string value)
            {
                return new RejectionReason(value: value);
            }

            ValidationError<GenericValidationErrorCode> CreateValidationError(
                string fieldId,
                GenericValidationErrorCode errorCode)
            {
                return new ValidationError<GenericValidationErrorCode>(
                    fieldId: fieldId, 
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