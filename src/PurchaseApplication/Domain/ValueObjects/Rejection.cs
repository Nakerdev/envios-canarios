using System;
using LanguageExt;
using static LanguageExt.Prelude;

namespace CanaryDeliveries.PurchaseApplication.Domain.ValueObjects
{
    public sealed class Rejection : Record<Rejection>
    {
        public PersistenceState State => new PersistenceState(dateTime: dateTime, reason: reason);
        
        private readonly DateTime dateTime;
        private readonly string reason;

        public static Validation<ValidationError<GenericValidationErrorCode>, Rejection> Create(
            Option<string> optionalDateTime,
            Option<string> optionalReason)
        {
            const string RejectionReasonFieldId = "RejectionReason";
            const string RejectionDateTime = "RejectionDateTime";
            
            return
                from datetime in ValidateRequire(RejectionDateTime, optionalDateTime)
                from reason in ValidateRequire(RejectionReasonFieldId, optionalReason)
                from formattedDateTime in ValidateDateTimeFormat(RejectionDateTime, datetime)
                from _1 in ValidateLenght(RejectionReasonFieldId, reason)
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
               string fieldId,
               string dateTime)
           {
               return parseDateTime(dateTime)
                   .ToValidation(CreateValidationError(
                        fieldId: fieldId,
                        errorCode: GenericValidationErrorCode.InvalidFormat));
           } 
            
            Validation<ValidationError<GenericValidationErrorCode>, Unit> ValidateLenght(
                string fieldId, 
                string reason)
            {
                const int maxAllowedLenght = 1000;
                if (reason.Length > maxAllowedLenght)
                {
                    return CreateValidationError(
                        fieldId: fieldId,
                        errorCode: GenericValidationErrorCode.WrongLength);
                }
                return unit;
            }
            
            Validation<ValidationError<GenericValidationErrorCode>, Rejection> BuildReject(
                DateTime dateTime,
                string reason)
            {
                return new Rejection(dateTime: dateTime, reason: reason);
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

        public Rejection(PersistenceState persistenceState)
        {
            dateTime = persistenceState.DateTime;
            reason = persistenceState.Reason;
        }

        private Rejection(DateTime dateTime, string reason)
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