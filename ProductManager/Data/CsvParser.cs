using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using ProductManager.Models;
using System.Globalization;

namespace ProductManager.Data
{
    public class CsvParser
    {
        public static List<Product> ParseCsv(string filePath)
        {
            var products = new List<Product>();

            try
            {
                using var reader = new StreamReader(filePath);
                using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    TrimOptions = TrimOptions.Trim,    // Trim whitespace
                    HeaderValidated = null,           // Ignore missing headers
                    MissingFieldFound = null          // Ignore missing fields
                });

                csv.Context.RegisterClassMap<ProductMap>();
                products = csv.GetRecords<Product>().ToList();
            }
            catch (TypeConverterException ex)
            {
                Console.WriteLine($"Data conversion error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading CSV: {ex.Message}");
            }

            return products;
        }
    }
}
