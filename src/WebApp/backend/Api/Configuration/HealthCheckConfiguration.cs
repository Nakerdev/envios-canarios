using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace CanaryDeliveries.WebApp.Api.Configuration
{
    public static class HealthCheckConfiguration
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddNpgSql(Environment.PurchaseApplicationDbConnectionString);
        }

        public static void ConfigureEndPoint(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHealthChecks("/health");
        }
    }
}