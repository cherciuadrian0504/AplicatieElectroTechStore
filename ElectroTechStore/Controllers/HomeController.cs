using System.Diagnostics;
using ElectroTechStore.Data;
using ElectroTechStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectroTechStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: / sau /Home/Index?categoryId=1&search=arduino&sort=price_asc
        public async Task<IActionResult> Index(int? categoryId, string? search, string? sort)
        {
            // Încărcăm toate categoriile împreună cu produsele pentru meniul lateral
            var categories = await _db.Categories
                .Include(c => c.Products)
                .OrderBy(c => c.Name)
                .ToListAsync();

            ViewBag.Categories = categories;
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.CurrentSearch = search;
            ViewBag.CurrentSort = sort;

            // Pornim interogarea pentru produse
            var productsQuery = _db.Products
                .Include(p => p.Category)
                .AsQueryable();

            // 1. Filtrare după categorie
            if (categoryId.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == categoryId.Value);
                var activeCat = categories.FirstOrDefault(c => c.Id == categoryId.Value);
                ViewBag.SelectedCategoryName = activeCat?.Name;
                ViewBag.SelectedCategoryDescription = activeCat?.Description;
            }

            // 2. Căutare după nume, descriere, specificații sau furnizor
            if (!string.IsNullOrWhiteSpace(search))
            {
                string term = search.Trim().ToLower();
                productsQuery = productsQuery.Where(p =>
                    p.Name.ToLower().Contains(term) ||
                    p.Description.ToLower().Contains(term) ||
                    p.Specifications.ToLower().Contains(term) ||
                    p.Supplier.ToLower().Contains(term));
            }

            // 3. Sortare produse
            productsQuery = sort switch
            {
                "price_asc" => productsQuery.OrderBy(p => p.Price),
                "price_desc" => productsQuery.OrderByDescending(p => p.Price),
                "stock_desc" => productsQuery.OrderByDescending(p => p.Stock),
                _ => productsQuery.OrderByDescending(p => p.Id)
            };

            var products = await productsQuery.ToListAsync();
            return View(products);
        }

        // GET: /Home/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _db.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            // Produse similare din aceeași categorie
            ViewBag.RelatedProducts = await _db.Products
                .Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id)
                .Take(3)
                .ToListAsync();

            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}