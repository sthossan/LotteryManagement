using Library.Core.Models.JwtModels;
using Microsoft.IdentityModel.Tokens;
using System;

namespace Library.Core.Infrastructure
{
    public static class JwtTokenValidationParameters
    {
        public static SecurityKey GetSymmetricSecurityKey()
        {
            byte[] symmetricKey = Convert.FromBase64String(JwtTokenConfig.TokenSecurityKey);
            return new SymmetricSecurityKey(symmetricKey);
        }

        public static TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters()
            {
                RequireExpirationTime = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = JwtTokenConfig.TokenIssuer,
                ValidAudience = JwtTokenConfig.TokenAudience,
                IssuerSigningKey = GetSymmetricSecurityKey(),
            };
        }
    }
}
