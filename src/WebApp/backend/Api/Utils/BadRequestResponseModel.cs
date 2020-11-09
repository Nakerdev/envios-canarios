using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CanaryDeliveries.WebApp.Api.Utils
{
    public sealed class BadRequestResponseModel
    {
        public List<ValidationError> ValidationErrors { get; }
        public string OperationError { get; }

        public static BadRequestResponseModel CreateValidationErrorResponse(
            List<ValidationError> validationErrors)
        {
            return new BadRequestResponseModel(
                validationErrors: validationErrors,
                operationError: null);
        }

        public static BadRequestResponseModel CreateOperationErrorResponse(
            string operationError)
        {
            return new BadRequestResponseModel(
                validationErrors: null,
                operationError: operationError);
        }

        private BadRequestResponseModel(
            List<ValidationError> validationErrors, 
            string operationError)
        {
            this.ValidationErrors = validationErrors;
            this.OperationError = operationError;
        }
    }

    public sealed class ValidationError
    {
        [Required]            
        public string FieldId { get; }

        [Required]            
        public string ErrorCode { get; }

        public ValidationError(
            string fieldId, 
            string errorCode)
        {
            FieldId = fieldId;
            ErrorCode = errorCode;
        }
    }
}