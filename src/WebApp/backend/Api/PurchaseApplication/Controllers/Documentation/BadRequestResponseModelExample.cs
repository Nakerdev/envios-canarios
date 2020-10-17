using System.Collections.Generic;
using CanaryDeliveries.WebApp.Api.Utils;
using Swashbuckle.AspNetCore.Filters;

namespace CanaryDeliveries.WebApp.Api.PurchaseApplication.Controllers.Documentation
{
    public sealed class BadRequestResponseModelExampleForValidationsError : IExamplesProvider<BadRequestResponseModel<PurchaseApplicationCreationRequestErrorCode>>
    {
        public BadRequestResponseModel<PurchaseApplicationCreationRequestErrorCode> GetExamples()
        {
            return BadRequestResponseModel<PurchaseApplicationCreationRequestErrorCode>.CreateValidationErrorResponse(
                validationErrors: new List<ValidationError<PurchaseApplicationCreationRequestErrorCode>>
                {
                    new ValidationError<PurchaseApplicationCreationRequestErrorCode>(fieldId: "Products", errorCode: PurchaseApplicationCreationRequestErrorCode.Required),
                    new ValidationError<PurchaseApplicationCreationRequestErrorCode>(fieldId: "Products[0].Link", errorCode: PurchaseApplicationCreationRequestErrorCode.Required),
                    new ValidationError<PurchaseApplicationCreationRequestErrorCode>(fieldId: "Products[0].Link", errorCode: PurchaseApplicationCreationRequestErrorCode.InvalidFormat),
                    new ValidationError<PurchaseApplicationCreationRequestErrorCode>(fieldId: "Products[0].Link", errorCode: PurchaseApplicationCreationRequestErrorCode.WrongLength),
                    new ValidationError<PurchaseApplicationCreationRequestErrorCode>(fieldId: "Products[0].Units", errorCode: PurchaseApplicationCreationRequestErrorCode.Required),
                    new ValidationError<PurchaseApplicationCreationRequestErrorCode>(fieldId: "Products[0].Units", errorCode: PurchaseApplicationCreationRequestErrorCode.InvalidFormat),
                    new ValidationError<PurchaseApplicationCreationRequestErrorCode>(fieldId: "Products[0].Units", errorCode: PurchaseApplicationCreationRequestErrorCode.InvalidValue),
                    new ValidationError<PurchaseApplicationCreationRequestErrorCode>(fieldId: "Products[0].AdditionalInformation", errorCode: PurchaseApplicationCreationRequestErrorCode.WrongLength),
                    new ValidationError<PurchaseApplicationCreationRequestErrorCode>(fieldId: "Products[0].PromotionCode", errorCode: PurchaseApplicationCreationRequestErrorCode.WrongLength),
                    new ValidationError<PurchaseApplicationCreationRequestErrorCode>(fieldId: "Client", errorCode: PurchaseApplicationCreationRequestErrorCode.Required),
                    new ValidationError<PurchaseApplicationCreationRequestErrorCode>(fieldId: "Client[0].Name", errorCode: PurchaseApplicationCreationRequestErrorCode.Required),
                    new ValidationError<PurchaseApplicationCreationRequestErrorCode>(fieldId: "Client[0].Name", errorCode: PurchaseApplicationCreationRequestErrorCode.WrongLength),
                    new ValidationError<PurchaseApplicationCreationRequestErrorCode>(fieldId: "Client[0].TelephoneNumber", errorCode: PurchaseApplicationCreationRequestErrorCode.Required),
                    new ValidationError<PurchaseApplicationCreationRequestErrorCode>(fieldId: "Client[0].TelephoneNumber", errorCode: PurchaseApplicationCreationRequestErrorCode.InvalidFormat),
                    new ValidationError<PurchaseApplicationCreationRequestErrorCode>(fieldId: "Client[0].TelephoneNumber", errorCode: PurchaseApplicationCreationRequestErrorCode.WrongLength),
                    new ValidationError<PurchaseApplicationCreationRequestErrorCode>(fieldId: "Client[0].Email", errorCode: PurchaseApplicationCreationRequestErrorCode.Required),
                    new ValidationError<PurchaseApplicationCreationRequestErrorCode>(fieldId: "Client[0].Email", errorCode: PurchaseApplicationCreationRequestErrorCode.InvalidFormat),
                    new ValidationError<PurchaseApplicationCreationRequestErrorCode>(fieldId: "Client[0].Email", errorCode: PurchaseApplicationCreationRequestErrorCode.WrongLength),
                    new ValidationError<PurchaseApplicationCreationRequestErrorCode>(fieldId: "AdditionalInformation", errorCode: PurchaseApplicationCreationRequestErrorCode.WrongLength),
                }
            );
        }
    }
}