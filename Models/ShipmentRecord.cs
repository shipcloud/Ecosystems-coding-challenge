using CsvHelper.Configuration.Attributes;

namespace Shipcloud.CodingChallenge.Models
{
    /// <summary>
    /// Represents a row from the Shipments CSV file.
    /// </summary>
    public class ShipmentRecord
    {
        [Name("DeliveryNumber")]
        public string DeliveryNumber { get; set; } = string.Empty;

        [Name("DeliveryPlanCode")]
        public string DeliveryPlanCode { get; set; } = string.Empty;

        [Name("ShipToAddressline1")]
        public string ShipToAddressline1 { get; set; } = string.Empty;

        [Name("ShipToAddressline2")]
        public string ShipToAddressline2 { get; set; } = string.Empty;

        [Name("ShipToAddressline3")]
        public string ShipToAddressline3 { get; set; } = string.Empty;

        [Name("ShipToStreet")]
        public string ShipToStreet { get; set; } = string.Empty;

        [Name("ShipToHouseNumber")]
        public string ShipToHouseNumber { get; set; } = string.Empty;

        [Name("ShipToCity")]
        public string ShipToCity { get; set; } = string.Empty;

        [Name("ShipToPostalCode")]
        public string ShipToPostalCode { get; set; } = string.Empty;

        [Name("ShipToCountry")]
        public string ShipToCountry { get; set; } = string.Empty;

        [Name("ParcelWeight")]
        public decimal ParcelWeight { get; set; }

        [Name("ParcelLength")]
        public decimal ParcelLength { get; set; }

        [Name("ParcelWidth")]
        public decimal ParcelWidth { get; set; }

        [Name("ParcelHeight")]
        public decimal ParcelHeight { get; set; }
    }
}
