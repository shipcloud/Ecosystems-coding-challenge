using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Shipcloud.CodingChallenge.Models;

namespace Shipcloud.CodingChallenge.Services
{
    /// <summary>
    /// Reads and parses the Shipments CSV file.
    /// </summary>
    public class CsvService
    {
        /// <summary>
        /// Reads all shipment records from the given CSV file path.
        /// </summary>
        public IEnumerable<ShipmentRecord> ReadShipments(string csvPath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true,
                PrepareHeaderForMatch = args => args.Header.Trim(),
            };

            using var reader = new StreamReader(csvPath);
            using var csv = new CsvReader(reader, config);

            return csv.GetRecords<ShipmentRecord>().ToList();
        }

        /// <summary>
        /// Groups shipment records by DeliveryNumber so that multi-colli shipments
        /// (same DeliveryNumber, multiple rows) are handled correctly.
        /// </summary>
        public Dictionary<string, List<ShipmentRecord>> GroupByDeliveryNumber(
            IEnumerable<ShipmentRecord> records)
        {
            return records
                .GroupBy(r => r.DeliveryNumber)
                .ToDictionary(g => g.Key, g => g.ToList());
        }
    }
}
