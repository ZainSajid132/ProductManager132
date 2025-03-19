using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductManager.Data;
using ProductManager.Models;

namespace ProductManager.Pages.Products
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly DataAccess _dataAccess;
        public CreateModel(DataAccess dataAccess) => _dataAccess = dataAccess;

        [BindProperty]
        public Product Product { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _dataAccess.InsertProductAsync(Product);
            return RedirectToPage("Index");
        }
    }
}
