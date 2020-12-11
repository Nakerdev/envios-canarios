using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CanaryDeliveries.Backoffice.Api.Users.Login.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace CanaryDeliveries.Backoffice.Api.Auth
{
    public sealed class JsonWebTokenHandler : TokenHandler
    {
        private readonly JwtSecurityTokenHandler tokenHandler;
        private readonly byte[] jsonWebTokenSecretKey;

        public JsonWebTokenHandler(byte[] jsonWebTokenSecretKey)
        {
            this.tokenHandler = new JwtSecurityTokenHandler();
            this.jsonWebTokenSecretKey = jsonWebTokenSecretKey;
        }

        public Token Create(BackofficeUser user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(jsonWebTokenSecretKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var createdToken = tokenHandler.CreateToken(tokenDescriptor);
            return new Token(value: tokenHandler.WriteToken(createdToken)); 
        }
    }
}