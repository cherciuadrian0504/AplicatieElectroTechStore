using Microsoft.AspNetCore.Mvc;

namespace ElectroTechStore.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
