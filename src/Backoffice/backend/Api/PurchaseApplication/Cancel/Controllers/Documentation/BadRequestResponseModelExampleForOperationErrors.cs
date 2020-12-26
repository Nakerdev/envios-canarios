using System.Collections.Generic;
using CanaryDeliveries.Backoffice.Api.Utils;
using CanaryDeliveries.PurchaseApplication.Domain.Cancel;
using Swashbuckle.AspNetCore.Filters;

namespace CanaryDeliveries.Backoffice.Api.PurchaseApplication.Cancel.Controllers.Documentation
{
    public sealed class BadRequestResponseModelExampleForOperation : IExamplesProvider<List<BadRequestResponseModel>>
    {
        public List<BadRequestResponseModel> GetExamples()
        {
            var examples = new List<BadRequestResponseModel>
            {
                BadRequestResponseModel.CreateOperationErrorResponse(Error.PurchaseApplicationNotFound.ToString()),
                BadRequestResponseModel.CreateOperationErrorResponse(Error.PurchaseApplicationIsAlreadyRejected.ToString())
            };
            return examples;
        }
    }
}