using Microsoft.AspNetCore.Mvc;

namespace AutoPartsShop.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
