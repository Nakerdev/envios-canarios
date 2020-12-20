using System;
using LanguageExt;
using static LanguageExt.Prelude;

namespace CanaryDeliveries.PurchaseApplication.Domain.ValueObjects
{
    public sealed class Reject : Record<Reject>
    {
        public PersistenceState State => new PersistenceState(dateTime: dateTime, reason: reason);
        
        private readonly DateTime dateTime;
        private readonly string reason;
        
        public static Validation<ValidationError<GenericValidationErrorCode>, Reject> Create(
            Option<string> optionalDateTime,
            Option<string> optionalReason)
        {
            return
                from datetime in ValidateRequire("RejectionDateTime", optionalDateTime)
                from reason in ValidateRequire("RejectionReason", optionalReason)
                from formattedDateTime in ValidateDateTimeFormat(datetime)
                from _1 in ValidateLenght(reason)
                from reject in BuildReject(formattedDateTime, reason)
                select reject;

            Validation<ValidationError<GenericValidationErrorCode>, string> ValidateRequire(
                string fieldId,
                Option<string> value)
            {
                return value
                    .ToValidation(CreateValidationError(
                        fieldId: fieldId,
                        errorCode: GenericValidationErrorCode.Required));
            }
            
           Validation<ValidationError<GenericValidationErrorCode>, DateTime> ValidateDateTimeFormat(
               string dateTime)
           {
               return parseDateTime(dateTime)
                   .ToValidation(CreateValidationError(
                        fieldId: "RejectionDateTime",
                        errorCode: GenericValidationErrorCode.InvalidFormat));
           } 
            
            Validation<ValidationError<GenericValidationErrorCode>, Unit> ValidateLenght(
                string reason)
            {
                const int maxAllowedLenght = 1000;
                if (reason.Length > maxAllowedLenght)
                {
                    return CreateValidationError(
                        fieldId: "RejectionReason",
                        errorCode: GenericValidationErrorCode.WrongLength);
                }
                return unit;
            }
            
            Validation<ValidationError<GenericValidationErrorCode>, Reject> BuildReject(
                DateTime dateTime,
                string reason)
            {
                return new Reject(dateTime: dateTime, reason: reason);
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

        public Reject(PersistenceState persistenceState)
        {
            dateTime = persistenceState.DateTime;
            reason = persistenceState.Reason;
        }

        private Reject(DateTime dateTime, string reason)
        {
            this.dateTime = dateTime;
            this.reason = reason;
        }

        public sealed class PersistenceState
        {
            public DateTime DateTime { get; }
            public string Reason { get; }

            public PersistenceState(DateTime dateTime, string reason)
            {
                DateTime = dateTime;
                Reason = reason;
            }
        }
    }
}