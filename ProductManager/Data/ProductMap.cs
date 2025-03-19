using CsvHelper.Configuration;
using ProductManager.Models;

namespace ProductManager.Data
{
    public class ProductMap : ClassMap<Product>
    {
        public ProductMap()
        {
            Map(m => m.Name).Name("Name"); // Ensure it matches exactly
            Map(m => m.Price).Name("Price");
            Map(m => m.Quantity).Name("Quantity");
        }
    }
}
