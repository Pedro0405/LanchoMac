using Microsoft.AspNetCore.Mvc;

namespace LanchoMac.Controllers
{
    public class ContatoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
