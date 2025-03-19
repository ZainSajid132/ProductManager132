using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProductManager.Data;
using ProductManager.Models;

namespace ProductManager.Pages.Products
{

    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly DataAccess _dataAccess;
        public IndexModel(DataAccess dataAccess) => _dataAccess = dataAccess;

        public List<Product> Products { get; set; }

        public async Task OnGetAsync()
        {
            Products = (await _dataAccess.GetAllProductsAsync()).AsList();
        }
    }
}
