using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace CanaryDeliveries.WebApp.Api.Configuration.Middlewares
{
    public static class HealthCheckMiddleware
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services
                .AddHealthChecks()
                .AddNpgSql(Environment.PurchaseApplicationDbConnectionString);
        }

        public static void ConfigureEndPoint(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHealthChecks("/health");
        }
    }
}