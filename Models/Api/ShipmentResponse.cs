using System.Text.Json.Serialization;

namespace Shipcloud.CodingChallenge.Models.Api
{
    public class ShipmentResponse
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("shipmentId")]
        public string? ShipmentId { get; set; }

        [JsonPropertyName("trackingNumber")]
        public string? TrackingNumber { get; set; }

        [JsonPropertyName("trackingNumbers")]
        public List<string>? TrackingNumbers { get; set; }

        [JsonPropertyName("label")]
        public string? Label { get; set; }

        [JsonPropertyName("labels")]
        public List<string>? Labels { get; set; }

        /// <summary>
        /// Returns the primary tracking number regardless of which property name was used.
        /// </summary>
        public string GetTrackingNumber() =>
            TrackingNumber
            ?? TrackingNumbers?.FirstOrDefault()
            ?? Id
            ?? ShipmentId
            ?? string.Empty;

        /// <summary>
        /// Returns all tracking numbers.
        /// </summary>
        public IEnumerable<string> GetAllTrackingNumbers()
        {
            if (TrackingNumbers?.Count > 0)
                return TrackingNumbers;
            if (TrackingNumber != null)
                return new[] { TrackingNumber };
            return Enumerable.Empty<string>();
        }

        /// <summary>
        /// Returns all label data.
        /// </summary>
        public IEnumerable<string> GetAllLabels()
        {
            if (Labels?.Count > 0)
                return Labels;
            if (Label != null)
                return new[] { Label };
            return Enumerable.Empty<string>();
        }
    }
}
