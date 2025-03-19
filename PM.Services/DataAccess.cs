﻿using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProductManager.Models;

namespace PM.Services
{
    public class DataAccess
    {
        private readonly string _connectionString;

        public DataAccess(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<Product>("sp_GetAllProducts", commandType: CommandType.StoredProcedure);
        }

        public async Task<int> InsertProductAsync(Product product)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync("sp_InsertProduct",
                new { product.Name, product.Price, product.Quantity },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> UpdateProductAsync(Product product)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync("sp_UpdateProduct",
                new { product.Id, product.Name, product.Price, product.Quantity },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> DeleteProductAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync("sp_DeleteProduct",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }
    }
}
