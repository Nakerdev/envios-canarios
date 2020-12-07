using CanaryDeliveries.WebApp.Api.Configuration;
using CanaryDeliveries.WebApp.Api.Configuration.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CanaryDeliveries.WebApp.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(config => {
                config.Filters.Add(new HttpResponseExceptionFilter());
            });
            services.AddMvc().AddJsonOptions(options => {
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });
            
            HealthCheckConfiguration.Configure(services);
            ApiControllersDependenciesResolver.Resolve(services);
            SwaggerConfiguration.ConfigureServices(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                SwaggerConfiguration.Configure(app);
            }
            else
            {
                app.UseHttpsRedirection();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                HealthCheckConfiguration.ConfigureEndPoint(endpoints);
            });
        }
    }
}
