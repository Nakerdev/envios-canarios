using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CanaryDeliveries.WebApp.Api.Utils
{
    public sealed class BadRequestResponseModel<ValidationErrorCode> where ValidationErrorCode : Enum
    {
        public List<ValidationError<ValidationErrorCode>> ValidationErrors { get; }
        public string OperationError { get; }

        public static BadRequestResponseModel<ValidationErrorCode> CreateValidationErrorResponse(
            List<ValidationError<ValidationErrorCode>> validationErrors)
        {
            return new BadRequestResponseModel<ValidationErrorCode>(
                validationErrors: validationErrors,
                operationError: null);
        }

        public static BadRequestResponseModel<ValidationErrorCode> CreateOperationErrorResponse(
            string operationError)
        {
            return new BadRequestResponseModel<ValidationErrorCode>(
                validationErrors: null,
                operationError: operationError);
        }

        private BadRequestResponseModel(
            List<ValidationError<ValidationErrorCode>> validationErrors, 
            string operationError)
        {
            this.ValidationErrors = validationErrors;
            this.OperationError = operationError;
        }
    }

    public sealed class ValidationError<ValidationErrorCode> where ValidationErrorCode : Enum
    {

        [Required]            
        public string FieldId { get; }

        [Required]            
        public string ErrorCode { get; }

        public ValidationError(
            string fieldId, 
            ValidationErrorCode errorCode)
        {
            this.FieldId = fieldId;
            this.ErrorCode = errorCode.ToString();
        }
    }
}