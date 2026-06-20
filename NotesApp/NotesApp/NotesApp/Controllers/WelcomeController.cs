using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NotesApp.DBLogic;
using NotesApp.Extensions;
using NotesApp.JwtLogic;
using NotesApp.Models;

namespace NotesApp.Controllers
{
    public class WelcomeController : Controller
    {
        private readonly JwtProvider jwtProvider;

        private readonly CryptoPassword cryptoPassword;

        private readonly DBConnection db;

        public WelcomeController(JwtProvider _jwtProvider, CryptoPassword _cryptoPassword, DBConnection _db)
        {
            jwtProvider = _jwtProvider;
            cryptoPassword = _cryptoPassword;
            db = _db;
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(RegModel userData)
        {
            if (ModelState.IsValid)
            {
                if (db.UsersInfo.FirstOrDefault(u => Equals(u.Email, userData.Email)) == null)
                {
                    userData.Password = cryptoPassword.HashPassword(userData.Password);
                    User user = new User() { UserName = userData.UserName, Email = userData.Email, Password = userData.Password };
                    db.UsersInfo.Add(user);
                    db.SaveChanges();
                    jwtProvider.SetAuthCookie(Response, userData.Email);
                    TempData["Message"] = "You have successfully registered\r\nWelcome";
                    return RedirectToAction("Success");
                }
                else
                {
                    ModelState.AddModelError("Email", "A user with this email already exsist");
                }
            }
            return View(userData);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel userData)
        {
            var user = db.UsersInfo.FirstOrDefault(u => Equals(u.Email, userData.Email));
            if (user != null)
            {
                if (cryptoPassword.ComparePasswords(user.Password, userData.Password))
                {
                    jwtProvider.SetAuthCookie(Response, userData.Email);
                    return RedirectToAction("Main", "Home");
                }
                else
                {
                    ModelState.AddModelError("Password", "The password is not correct");
                }
            }
            else
            {
                ModelState.AddModelError("Email", "This user don't exsist");
            }
            return View(userData);
        }

        [HttpGet]
        public IActionResult Success()
        {
            var message = TempData["Message"];
            return View(model: message);
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("strange-cookie");
            return RedirectToAction("Login");
        }
    }
}
