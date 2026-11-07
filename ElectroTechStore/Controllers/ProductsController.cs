using ElectroTechStore.Data;
using ElectroTechStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ElectroTechStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: /Products
        public async Task<IActionResult> Index()
        {
            var products = await _db.Products
                .Include(p => p.Category)
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            return View(products);
        }

        // GET: /Products/Create
        public async Task<IActionResult> Create()
        {
            await PopulateCategoriesDropdownAsync();
            return View();
        }

        // POST: /Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                await PopulateCategoriesDropdownAsync(product.CategoryId);
                return View(product);
            }

            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Produsul '{product.Name}' a fost adăugat în catalog.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();

            await PopulateCategoriesDropdownAsync(product.CategoryId);
            return View(product);
        }

        // POST: /Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                await PopulateCategoriesDropdownAsync(product.CategoryId);
                return View(product);
            }

            _db.Entry(product).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Produsul '{product.Name}' a fost actualizat.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = await _db.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }

        // POST: /Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product != null)
            {
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
                TempData["SuccessMessage"] = "Produsul a fost șters din catalog.";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateCategoriesDropdownAsync(object? selectedCategory = null)
        {
            var categories = await _db.Categories.OrderBy(c => c.Name).ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", selectedCategory);
        }
    }
}