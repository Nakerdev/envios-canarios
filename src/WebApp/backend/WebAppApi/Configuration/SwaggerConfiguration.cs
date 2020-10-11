using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace WebAppApi.Configuration
{
    public static class SwaggerConfiguration
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c => {
                c.EnableAnnotations();
            });
        }

        public static void Configure(IApplicationBuilder app) 
        {
                app.UseSwagger();
                app.UseSwaggerUI(c => 
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Envios Canarios Web App Api");
                    c.RoutePrefix = "_doc";
                });             
        }
    }
}