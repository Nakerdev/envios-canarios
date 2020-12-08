using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Filters;

namespace CanaryDeliveries.WebApp.Api.Configuration.Middlewares
{
    public static class SwaggerMiddleware
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c => {
                c.EnableAnnotations();
                c.CustomSchemaIds(schemaIdStrategy);
                c.ExampleFilters();
            });
            services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
        }

        public static void ConfigureApplication(IApplicationBuilder app) 
        {
                app.UseSwagger();
                app.UseSwaggerUI(c => 
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Canary Deliveries Web App Api");
                    c.RoutePrefix = "_doc";
                });             
        }

        private static string schemaIdStrategy(Type currentClass)
        {
            if(currentClass.IsGenericType){
                return currentClass.Name.Remove(currentClass.Name.IndexOf('`'));
            }
            return currentClass.Name;
        }
    }
}