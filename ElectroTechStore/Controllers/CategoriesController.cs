using Microsoft.AspNetCore.Mvc;

namespace ElectroTechStore.Controllers
{
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
