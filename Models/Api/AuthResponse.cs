using System.Text.Json.Serialization;

namespace Shipcloud.CodingChallenge.Models.Api
{
    public class AuthResponse
    {
        [JsonPropertyName("token")]
        public string? Token { get; set; }

        [JsonPropertyName("accessToken")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("jwt")]
        public string? Jwt { get; set; }

        /// <summary>
        /// Returns the authentication token regardless of which property name was used.
        /// </summary>
        public string GetToken() =>
            Token ?? AccessToken ?? Jwt ?? string.Empty;
    }
}
