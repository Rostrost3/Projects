using Microsoft.AspNetCore.Mvc;

namespace Registration.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Abouts()
        {
            return View();
        }
    }
}
