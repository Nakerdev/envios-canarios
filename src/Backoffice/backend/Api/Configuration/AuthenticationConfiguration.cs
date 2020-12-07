using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CanaryDeliveries.Backoffice.Api.Configuration
{
    public static class AuthenticationConfiguration
    {
        public static void Configure(IServiceCollection services)
        {
             services.AddAuthentication(x =>
             {
                 x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                 x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
             }).AddJwtBearer(x =>
             {
                 x.RequireHttpsMetadata = false;
                 x.SaveToken = true;
                 x.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(Environment.JsonWebTokenSecretKey),
                     ValidateIssuer = false,
                     ValidateAudience = false
                 };
             });           
        }
    }
}