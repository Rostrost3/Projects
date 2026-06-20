using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesApp.DBLogic;

namespace NotesApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly DBConnection db;

        public HomeController(DBConnection _db)
        {
            db = _db;
        }

        public IActionResult Main()
        {
            string email = User.Claims.First(c => c.Type == "UserMail").Value;
            var user = db.UsersInfo.First(x => x.Email == email);
            var userNotes = db.Notes.Where(n => n.UserId == user.Id).ToList();
            return View(userNotes);
        }
    }
}