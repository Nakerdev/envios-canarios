using CanaryDeliveries.Backoffice.Api.Utils;
using CanaryDeliveries.PurchaseApplication.Domain.Cancel;
using CanaryDeliveries.PurchaseApplication.Domain.ValueObjects;
using LanguageExt;
using Microsoft.AspNetCore.Mvc;

namespace CanaryDeliveries.Backoffice.Api.PurchaseApplication.Cancel.Controllers
{
    public sealed class CancelPurchaseApplicationController : ControllerBase
    {
        private readonly CancelPurchaseApplicationCommandHandler commandHandler;

        public CancelPurchaseApplicationController(CancelPurchaseApplicationCommandHandler commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        public ActionResult Cancel(RequestDto request)
        {
            var command = BuildCancelPurchaseApplicationCommand(request);
            return command.Match(
                Fail: BuildValidationErrorResponse,
                Succ: ExecuteCommandHandler);
        }

        private static Validation<
            ValidationError<GenericValidationErrorCode>, 
            CancelPurchaseApplicationCommand> BuildCancelPurchaseApplicationCommand(RequestDto request)
        {
            var dto = new CancelPurchaseApplicationCommand.Dto(
                purchaseApplicationId: request.PurchaseApplicationId,
                rejectionReason: request.RejectionReason);
            return CancelPurchaseApplicationCommand.Create(dto);
        }
        
        private ActionResult BuildValidationErrorResponse(Seq<ValidationError<GenericValidationErrorCode>> errors)
        {
            var validationErrors = errors.Map(error => 
                new ValidationError(
                    fieldId: error.FieldId,
                    errorCode: error.ErrorCode.ToString()))
                .ToList();
            return BadRequest(BadRequestResponseModel.CreateValidationErrorResponse(validationErrors));
        }
        
        private ActionResult ExecuteCommandHandler(CancelPurchaseApplicationCommand command)
        {
            return commandHandler
                .Cancel(command)
                .Match(
                    Left: error => BadRequest(BadRequestResponseModel.CreateOperationErrorResponse(error.ToString())),
                    Right: _ => (ActionResult) Ok());
        }

        public sealed class RequestDto
        {
            public string PurchaseApplicationId { get; set; }
            public string RejectionReason { get; set; }
        }
    }
}