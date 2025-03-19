using System.Data;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProductManager.Data;
using ProductManager.Models;
using CsvHelper;
using Microsoft.AspNetCore.Authorization;

namespace ProductManager.Controllers
{
    [Route("api/products")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly string _connectionString;

        public ProductController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts(string? name = null, int? quantity = null)
        {
            using var connection = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@Name", name);
            parameters.Add("@Quantity", quantity);

            var products = await connection.QueryAsync<Product>(
                "sp_GetFilteredProducts",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return Ok(products);
        }


        [HttpPost("upload-csv")]
        public async Task<IActionResult> UploadCsv(IFormFile file, [FromServices] DataAccess dataAccess)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var products = new List<Product>();

            try
            {
                using var stream = file.OpenReadStream();
                using var reader = new StreamReader(stream, new UTF8Encoding(true)); // Handles BOM
                using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    TrimOptions = TrimOptions.Trim,
                    HeaderValidated = null,
                    MissingFieldFound = null,
                    BadDataFound = null,
                    DetectColumnCountChanges = true  // Helps catch incorrect headers
                });

                csv.Context.RegisterClassMap<ProductMap>();

                // Debugging: Print Headers
                csv.Read();
                csv.ReadHeader();
                var headers = csv.HeaderRecord;
                Console.WriteLine($"CSV Headers: {string.Join(", ", headers)}");

                // Read records
                products = csv.GetRecords<Product>().ToList();
                Console.WriteLine($"Products Count: {products.Count}");

                if (products.Count == 0)
                {
                    return BadRequest("No valid products found in the CSV.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error parsing CSV: {ex.Message}");
            }

            // Insert data using DataAccess
            try
            {
                int rowsInserted = await dataAccess.BulkInsertProductsAsync(products);
                return Ok(new { message = $"CSV uploaded successfully. {rowsInserted} records inserted." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error saving data: {ex.Message}");
            }
        }
    }
}
