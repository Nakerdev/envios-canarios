using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAppApi.Configuration.Filters
{
    public class HttpResponseExceptionFilter : ExceptionFilterAttribute 
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is System.Exception)
            {
                context.Result = new JsonResult(value: "Application error");
                context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            }
            base.OnException(context);
        }
    }
}