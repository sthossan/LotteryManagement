using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace Library.Core.Models.JwtModels
{
    public static class JwtTokenConfig
    {
        [JsonPropertyName("secret")]
        public static string TokenSecurityKey { get; } = "QS1WRVJZLVNUUk9ORy1LRVktSEVSRS1UQU5WSVJIT1NTQU4tR0FLSy1NRURJQS1CRC1MVEQ=";

        [JsonPropertyName("issuer")]
        public static string TokenIssuer { get; } = "Boilerplate";

        [JsonPropertyName("audience")]
        public static string TokenAudience { get; } = "Boilerplate";

        [JsonPropertyName("accessTokenExpiration")]
        public static int TokenExpiryTime { get; set; } = 10;
        
        [JsonPropertyName("refreshTokenExpiration")]
        public static int RefreshTokenExpiryTime { get; set; } = 10;

        public static string SecurityAlgorithm { get; } = SecurityAlgorithms.HmacSha512Signature;

        public static List<Claim> Claims { get; }
    }
}
