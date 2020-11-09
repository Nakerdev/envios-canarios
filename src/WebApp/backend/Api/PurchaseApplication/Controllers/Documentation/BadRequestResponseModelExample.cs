using System.Collections.Generic;
using CanaryDeliveries.WebApp.Api.Utils;
using Swashbuckle.AspNetCore.Filters;

namespace CanaryDeliveries.WebApp.Api.PurchaseApplication.Controllers.Documentation
{
    public sealed class BadRequestResponseModelExampleForValidationsError : IExamplesProvider<BadRequestResponseModel>
    {
        public BadRequestResponseModel GetExamples()
        {
            return BadRequestResponseModel.CreateValidationErrorResponse(
                validationErrors: new List<ValidationError>
                {
                    new ValidationError(fieldId: "Products", errorCode: "Required"),
                    new ValidationError(fieldId: "Products[0].Link", errorCode: "Required"),
                    new ValidationError(fieldId: "Products[0].Link", errorCode: "InvalidFormat"),
                    new ValidationError(fieldId: "Products[0].Link", errorCode: "WrongLength"),
                    new ValidationError(fieldId: "Products[0].Units", errorCode: "Required"),
                    new ValidationError(fieldId: "Products[0].Units", errorCode: "InvalidFormat"),
                    new ValidationError(fieldId: "Products[0].Units", errorCode: "InvalidValue"),
                    new ValidationError(fieldId: "Products[0].AdditionalInformation", errorCode: "WrongLength"),
                    new ValidationError(fieldId: "Products[0].PromotionCode", errorCode: "WrongLength"),
                    new ValidationError(fieldId: "Client", errorCode: "Required"),
                    new ValidationError(fieldId: "Client.Name", errorCode: "Required"),
                    new ValidationError(fieldId: "Client.Name", errorCode: "WrongLength"),
                    new ValidationError(fieldId: "Client.TelephoneNumber", errorCode: "Required"),
                    new ValidationError(fieldId: "Client.TelephoneNumber", errorCode: "InvalidFormat"),
                    new ValidationError(fieldId: "Client.TelephoneNumber", errorCode: "WrongLength"),
                    new ValidationError(fieldId: "Client.Email", errorCode: "Required"),
                    new ValidationError(fieldId: "Client.Email", errorCode: "InvalidFormat"),
                    new ValidationError(fieldId: "Client.Email", errorCode: "WrongLength"),
                    new ValidationError(fieldId: "AdditionalInformation", errorCode: "WrongLength"),
                }
            );
        }
    }
}