using System.Text.Json.Serialization;

namespace Shipcloud.CodingChallenge.Models.Api
{
    public class AuthRequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;
    }
}
