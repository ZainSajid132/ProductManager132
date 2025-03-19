using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductManager.Data;
using ProductManager.Models;

namespace ProductManager.Pages.Products
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly DataAccess _dataAccess;
        public EditModel(DataAccess dataAccess) => _dataAccess = dataAccess;

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
            if (!ModelState.IsValid) return Page();

            await _dataAccess.UpdateProductAsync(Product);
            return RedirectToPage("Index");
        }
    }
}
