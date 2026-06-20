using Microsoft.AspNetCore.Mvc;

namespace EncryptionProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Welcome()
        {
            return View();
        }
    }
}
