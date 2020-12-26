using System.Collections.Generic;
using CanaryDeliveries.Backoffice.Api.Utils;
using CanaryDeliveries.PurchaseApplication.Domain.Cancel;
using Swashbuckle.AspNetCore.Filters;

namespace CanaryDeliveries.Backoffice.Api.PurchaseApplication.Cancel.Controllers.Documentation
{
    public sealed class BadRequestResponseModelExample : IExamplesProvider<List<BadRequestResponseModel>>
    {
        public List<BadRequestResponseModel> GetExamples()
        {
            var examples = new List<BadRequestResponseModel>
            {
                BadRequestResponseModel.CreateValidationErrorResponse(
                    validationErrors: new List<ValidationError>
                    {
                        new ValidationError(fieldId: "Id", errorCode: "Required"),
                        new ValidationError(fieldId: "Id", errorCode: "InvalidFormat"),
                        new ValidationError(fieldId: "RejectionReason", errorCode: "Required"),
                        new ValidationError(fieldId: "RejectionReason", errorCode: "WrongLength")
                    }),
                BadRequestResponseModel.CreateOperationErrorResponse(Error.PurchaseApplicationNotFound.ToString()),
                BadRequestResponseModel.CreateOperationErrorResponse(Error.PurchaseApplicationIsAlreadyRejected.ToString())
            };
            return examples;
        }
    }
}