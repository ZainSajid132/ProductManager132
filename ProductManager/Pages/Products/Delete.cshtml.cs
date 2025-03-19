using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProductManager.Data;
using ProductManager.Models;

namespace ProductManager.Pages.Products
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly DataAccess _dataAccess;
        public DeleteModel(DataAccess dataAccess) => _dataAccess = dataAccess;

        [BindProperty]
        public Product Product { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _dataAccess.GetProductByIdAsync(id);
            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Product.Id == 0)  
            {
                return NotFound();
            }
            await _dataAccess.DeleteProductAsync(Product.Id);
            return RedirectToPage("Index");
        }
    }
}
