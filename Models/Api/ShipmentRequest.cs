using System.Text.Json.Serialization;

namespace Shipcloud.CodingChallenge.Models.Api
{
    public class Address
    {
        [JsonPropertyName("name1")]
        public string Name1 { get; set; } = string.Empty;

        [JsonPropertyName("name2")]
        public string? Name2 { get; set; }

        [JsonPropertyName("name3")]
        public string? Name3 { get; set; }

        [JsonPropertyName("street")]
        public string Street { get; set; } = string.Empty;

        [JsonPropertyName("houseNumber")]
        public string HouseNumber { get; set; } = string.Empty;

        [JsonPropertyName("city")]
        public string City { get; set; } = string.Empty;

        [JsonPropertyName("zipCode")]
        public string ZipCode { get; set; } = string.Empty;

        [JsonPropertyName("countryCode")]
        public string CountryCode { get; set; } = string.Empty;
    }

    public class Parcel
    {
        [JsonPropertyName("weight")]
        public decimal Weight { get; set; }

        [JsonPropertyName("length")]
        public decimal Length { get; set; }

        [JsonPropertyName("width")]
        public decimal Width { get; set; }

        [JsonPropertyName("height")]
        public decimal Height { get; set; }
    }

    public class ShipmentRequest
    {
        [JsonPropertyName("customerAccountCode")]
        public string CustomerAccountCode { get; set; } = "SHIPCL";

        [JsonPropertyName("deliveryPlanCode")]
        public string DeliveryPlanCode { get; set; } = string.Empty;

        [JsonPropertyName("senderAddress")]
        public Address SenderAddress { get; set; } = new();

        [JsonPropertyName("recipientAddress")]
        public Address RecipientAddress { get; set; } = new();

        [JsonPropertyName("parcels")]
        public List<Parcel> Parcels { get; set; } = new();
    }
}
