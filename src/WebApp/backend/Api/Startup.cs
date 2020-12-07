using CanaryDeliveries.PurchaseApplication.Domain.Create;
using CanaryDeliveries.PurchaseApplication.Domain.Services;
using CanaryDeliveries.PurchaseApplication.Repositories;
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
            services.AddScoped(provider => new CreatePurchaseApplicationCommandHandler(
                purchaseApplicationRepository: new PurchaseApplicationEntityFrameworkRepository(),
                timeService: new SystemTimeService()));
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
