using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Architecture.Logs;

namespace WebAppApi.Configuration.Filters
{
    public class HttpResponseExceptionFilter : ExceptionFilterAttribute 
    {
        private Logger logger = NLogLogger.Create(typeof(HttpResponseExceptionFilter));

        public override void OnException(ExceptionContext context)
        {
            logger.LogError(context.Exception, "Application error");
            context.Result = new JsonResult(value: "Application error");
            context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            base.OnException(context);
        }
    }
}