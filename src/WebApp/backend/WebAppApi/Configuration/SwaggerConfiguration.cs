using Microsoft.AspNetCore.Builder;

namespace WebAppApi.Configuration
{
    public static class SwaggerConfiguration
    {
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