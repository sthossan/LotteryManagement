using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace Library.Core.Models.JwtModels
{
    public static class JwtTokenConfig
    {
        [JsonPropertyName("secret")]
        public static string TokenSecurityKey { get; } = "a brown fox jumps over the lazy dog";

        [JsonPropertyName("issuer")]
        public static string TokenIssuer { get; } = "Boilerplate";

        [JsonPropertyName("audience")]
        public static string TokenAudience { get; } = "Boilerplate";

        [JsonPropertyName("accessTokenExpiration")]
        public static int TokenExpiryTime { get; set; } = 10;
        
        [JsonPropertyName("refreshTokenExpiration")]
        public static int RefreshTokenExpiryTime { get; set; } = 60;

        public static string SecurityAlgorithm { get; } = SecurityAlgorithms.HmacSha512Signature;

        public static List<Claim> Claims { get; }
    }
}
