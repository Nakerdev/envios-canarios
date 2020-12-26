using System.Collections.Generic;
using CanaryDeliveries.Backoffice.Api.Utils;
using Swashbuckle.AspNetCore.Filters;

namespace CanaryDeliveries.Backoffice.Api.PurchaseApplication.Cancel.Controllers.Documentation
{
    public sealed class BadRequestResponseModelExampleForValidationsError : IExamplesProvider<BadRequestResponseModel>
    {
        public BadRequestResponseModel GetExamples()
        {
            return BadRequestResponseModel.CreateValidationErrorResponse(
                validationErrors: new List<ValidationError>
                {
                    new ValidationError(fieldId: "Id", errorCode: "Required"),
                    new ValidationError(fieldId: "Id", errorCode: "InvalidFormat"),
                    new ValidationError(fieldId: "RejectionReason", errorCode: "Required"),
                    new ValidationError(fieldId: "RejectionReason", errorCode: "WrongLength")
                }
            );
        }
    }
}