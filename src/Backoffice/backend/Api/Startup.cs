using CanaryDeliveries.Backoffice.Api.Configuration.Filters;
using CanaryDeliveries.Backoffice.Api.Configuration.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AuthenticationMiddleware = CanaryDeliveries.Backoffice.Api.Configuration.Middleware.AuthenticationMiddleware;

namespace CanaryDeliveries.Backoffice.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            AuthenticationMiddleware.Configure(services);
            services.AddControllers(config => {
                config.Filters.Add(new HttpResponseExceptionFilter());
                config.Filters.Add(new AuthorizeFilter());
            });
            services.AddMvc().AddJsonOptions(options => {
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });
            ApiControllersDependenciesMiddleware.ConfigureServices(services);
            SwaggerMiddleware.ConfigureServices(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                SwaggerMiddleware.ConfigureApplication(app);
            }
            else
            {
                app.UseHttpsRedirection();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}