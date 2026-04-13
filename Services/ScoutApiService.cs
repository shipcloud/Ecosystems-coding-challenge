using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Shipcloud.CodingChallenge.Models.Api;

namespace Shipcloud.CodingChallenge.Services
{
    /// <summary>
    /// Communicates with the Shipcloud SCOUT API.
    /// </summary>
    public class ScoutApiService
    {
        private readonly HttpClient _httpClient;
        private string _authToken = string.Empty;

        // Known API endpoint paths - adjust if the actual swagger reveals different paths
        private const string LoginPath = "/rest/authenticate";
        private const string ShipmentsPath = "/rest/shipments";

        public ScoutApiService(string baseUrl)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl.TrimEnd('/'))
            };
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Authenticates with the SCOUT API and stores the JWT token for subsequent calls.
        /// </summary>
        public async Task AuthenticateAsync(string email, string password)
        {
            var request = new AuthRequest { Email = email, Password = password };

            var response = await _httpClient.PostAsJsonAsync(LoginPath, request);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException(
                    $"Authentication failed ({(int)response.StatusCode}): {body}");
            }

            var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
            _authToken = authResponse?.GetToken()
                ?? throw new InvalidOperationException(
                    "Authentication succeeded but no token was returned.");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _authToken);
        }

        /// <summary>
        /// Books a single shipment and returns the response containing the tracking number.
        /// </summary>
        public async Task<ShipmentResponse> BookShipmentAsync(ShipmentRequest shipmentRequest)
        {
            var response = await _httpClient.PostAsJsonAsync(ShipmentsPath, shipmentRequest);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException(
                    $"Failed to book shipment ({(int)response.StatusCode}): {body}");
            }

            var result = await response.Content.ReadFromJsonAsync<ShipmentResponse>();
            return result ?? throw new InvalidOperationException(
                "Shipment booking succeeded but no response body was returned.");
        }

        /// <summary>
        /// Downloads label bytes from the provided URL or base-64-encoded string.
        /// </summary>
        public async Task<byte[]> DownloadLabelAsync(string labelUrlOrBase64)
        {
            // If it looks like a URL, download it; otherwise treat as base64
            if (Uri.TryCreate(labelUrlOrBase64, UriKind.Absolute, out _))
            {
                return await _httpClient.GetByteArrayAsync(labelUrlOrBase64);
            }

            return Convert.FromBase64String(labelUrlOrBase64);
        }
    }
}
