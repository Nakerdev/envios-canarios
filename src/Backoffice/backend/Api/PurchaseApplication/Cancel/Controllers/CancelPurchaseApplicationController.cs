using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using CanaryDeliveries.Backoffice.Api.PurchaseApplication.Cancel.Controllers.Documentation;
using CanaryDeliveries.Backoffice.Api.Utils;
using CanaryDeliveries.PurchaseApplication.Domain.Cancel;
using CanaryDeliveries.PurchaseApplication.Domain.ValueObjects;
using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace CanaryDeliveries.Backoffice.Api.PurchaseApplication.Cancel.Controllers
{
    [ApiController]
    [Route("/v1/purchase-application/cancel")]
    public sealed class CancelPurchaseApplicationController : ControllerBase
    {
        private readonly CancelPurchaseApplicationCommandHandler commandHandler;

        public CancelPurchaseApplicationController(CancelPurchaseApplicationCommandHandler commandHandler)
        {
            this.commandHandler = commandHandler;
        }
        
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerOperation(summary: "Cancel specific purchase application", Tags = new []{"Purchase Applications"})]
        [SwaggerResponse(statusCode: 200, description: "Purchase application was cancelled successfully")]
        [SwaggerResponse(statusCode: 400, description: "The purchase application cancellation request has validation errors or an operation error occurs", type: typeof(BadRequestResponseModel))]
        [SwaggerResponse(statusCode: 401, description: "Unauthorized request")]
        [SwaggerResponse(statusCode: 500, description: "Unhandled error")]
        [SwaggerResponseExample(400, typeof(BadRequestResponseModelExample))]
        public ActionResult Cancel(CancelRequestDto request)
        {
            var command = BuildCancelPurchaseApplicationCommand(request);
            return command.Match(
                Fail: BuildValidationErrorResponse,
                Succ: ExecuteCommandHandler);
        }

        private static Validation<
            ValidationError<GenericValidationErrorCode>, 
            CancelPurchaseApplicationCommand> BuildCancelPurchaseApplicationCommand(CancelRequestDto request)
        {
            var dto = new CancelPurchaseApplicationCommand.Dto(
                purchaseApplicationId: request.Id,
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

        public sealed class CancelRequestDto
        {
            [SwaggerSchema("The purchase application identifier")] 
            [Required]            
            public string Id { get; set; }
            
            [SwaggerSchema("The purchase application identifier", Description = "Must contains a maximum of 1000 characters")] 
            [Required]            
            public string RejectionReason { get; set; }
        }
    }
}