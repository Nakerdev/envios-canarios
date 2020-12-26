using System;
using CanaryDeliveries.PurchaseApplication.Domain.Cancel;
using Microsoft.AspNetCore.Mvc;

namespace CanaryDeliveries.Backoffice.Api.PurchaseApplication.Cancel.Controllers
{
    public sealed class CancelPurchaseApplicationController 
    {
        private readonly CancelPurchaseApplicationCommandHandler commandHandler;

        public CancelPurchaseApplicationController(CancelPurchaseApplicationCommandHandler commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        public ActionResult Cancel(RequestDto request)
        {
            var command = CancelPurchaseApplicationCommand.Create(new CancelPurchaseApplicationCommand.Dto(
                    purchaseApplicationId: request.PurchaseApplicationId,
                    rejectionReason: request.RejectionReason))
                .IfFail(() => throw new NotImplementedException());
            return commandHandler
                .Cancel(command)
                .Match(
                    Left: _ => throw new NotImplementedException(),
                    Right: _ => new OkResult());
        }

        public sealed class RequestDto
        {
            public string PurchaseApplicationId { get; set; }
            public string RejectionReason { get; set; }
        }
    }
}