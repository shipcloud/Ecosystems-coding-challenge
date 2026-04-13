using Shipcloud.CodingChallenge.Models.Api;
using Shipcloud.CodingChallenge.Services;

namespace Shipcloud.CodingChallenge
{
    internal class Program
    {
        /// <summary>
        /// Shipment Booking System — reads shipments from a CSV file, books them via the
        /// Shipcloud SCOUT API, and prints the resulting tracking numbers.
        /// </summary>
        /// <param name="email">API authentication e-mail address.</param>
        /// <param name="password">API authentication password.</param>
        /// <param name="csvPath">Path to the shipments CSV file.</param>
        /// <param name="apiUrl">Base URL of the SCOUT API.</param>
        /// <param name="saveLabels">When set, download and save shipment labels to disk.</param>
        static async Task Main(
            string email,
            string password,
            string csvPath = "Resources/Shipments.csv",
            string apiUrl = "https://tms-staging.europaket.plus",
            bool saveLabels = false)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                Console.Error.WriteLine("Error: --email and --password are required.");
                Environment.Exit(1);
            }

            // ----------------------------------------------------------------
            // 1. Parse the CSV file
            // ----------------------------------------------------------------
            Console.WriteLine($"Reading shipments from: {csvPath}");

            var csvService = new CsvService();
            var records = csvService.ReadShipments(csvPath);
            var grouped = csvService.GroupByDeliveryNumber(records);

            Console.WriteLine($"Found {grouped.Count} shipment(s) " +
                              $"({records.Count()} parcel row(s)).");

            // ----------------------------------------------------------------
            // 2. Authenticate with the SCOUT API
            // ----------------------------------------------------------------
            Console.WriteLine("Authenticating with SCOUT API...");

            using var apiService = new ScoutApiService(apiUrl);
            await apiService.AuthenticateAsync(email, password);

            Console.WriteLine("Authentication successful.");

            // Sender address — the Shipcloud office in Madrid
            var senderAddress = new Address
            {
                Name1 = "Shipcloud Madrid",
                Street = "P.º de la Castellana",
                HouseNumber = "163",
                City = "Madrid",
                ZipCode = "28046",
                CountryCode = "ES"
            };

            // ----------------------------------------------------------------
            // 3. Book all shipments and collect results
            // ----------------------------------------------------------------
            var results = new List<(string DeliveryNumber, ShipmentResponse Response)>();

            foreach (var (deliveryNumber, parcels) in grouped)
            {
                Console.WriteLine($"Booking shipment {deliveryNumber} " +
                                  $"({parcels.Count} parcel(s))...");

                var first = parcels[0];

                var recipientAddress = new Address
                {
                    Name1 = first.ShipToAddressline1,
                    Name2 = NullIfWhitespace(first.ShipToAddressline2),
                    Name3 = NullIfWhitespace(first.ShipToAddressline3),
                    Street = first.ShipToStreet,
                    HouseNumber = first.ShipToHouseNumber,
                    City = first.ShipToCity,
                    ZipCode = first.ShipToPostalCode,
                    CountryCode = first.ShipToCountry
                };

                var shipmentRequest = new ShipmentRequest
                {
                    CustomerAccountCode = "SHIPCL",
                    DeliveryPlanCode = first.DeliveryPlanCode,
                    SenderAddress = senderAddress,
                    RecipientAddress = recipientAddress,
                    Parcels = parcels.Select(p => new Parcel
                    {
                        Weight = p.ParcelWeight,
                        Length = p.ParcelLength,
                        Width = p.ParcelWidth,
                        Height = p.ParcelHeight
                    }).ToList()
                };

                var response = await apiService.BookShipmentAsync(shipmentRequest);
                results.Add((deliveryNumber, response));

                Console.WriteLine($"  → Booked successfully.");
            }

            // ----------------------------------------------------------------
            // 4. Print all tracking numbers AFTER all shipments are booked
            // ----------------------------------------------------------------
            Console.WriteLine();
            Console.WriteLine("=== Tracking Numbers ===");
            foreach (var (deliveryNumber, response) in results)
            {
                var trackingNumbers = response.GetAllTrackingNumbers().ToList();
                if (trackingNumbers.Count == 0)
                {
                    Console.WriteLine($"{deliveryNumber} → {response.GetTrackingNumber()}");
                }
                else if (trackingNumbers.Count == 1)
                {
                    Console.WriteLine($"{deliveryNumber} → {trackingNumbers[0]}");
                }
                else
                {
                    Console.WriteLine($"{deliveryNumber} →");
                    foreach (var tn in trackingNumbers)
                        Console.WriteLine($"    {tn}");
                }
            }

            // ----------------------------------------------------------------
            // 5. Optionally save labels to disk
            // ----------------------------------------------------------------
            if (saveLabels)
            {
                var labelsDir = "Labels";
                Directory.CreateDirectory(labelsDir);

                Console.WriteLine();
                Console.WriteLine($"Saving labels to './{labelsDir}/'...");

                foreach (var (deliveryNumber, response) in results)
                {
                    var labelData = response.GetAllLabels().ToList();
                    for (int i = 0; i < labelData.Count; i++)
                    {
                        var suffix = labelData.Count > 1 ? $"_{i + 1}" : string.Empty;
                        var fileName = Path.Combine(labelsDir,
                            $"{deliveryNumber}{suffix}.pdf");

                        var bytes = await apiService.DownloadLabelAsync(labelData[i]);
                        await File.WriteAllBytesAsync(fileName, bytes);
                        Console.WriteLine($"  Saved: {fileName}");
                    }
                }
            }
        }

        private static string? NullIfWhitespace(string value) =>
            string.IsNullOrWhiteSpace(value) ? null : value;
    }
}
