using Microsoft.AspNetCore.Mvc;
using Registration.Context;
using Registration.Crypt;
using Registration.JWTLogic;
using Registration.Models;

namespace Registration.Controllers
{
    public class RegistrationUserController : Controller
    {
        CryptPass crypt;
        private readonly JWTProvider JwtProvider;

        public RegistrationUserController(JWTProvider _JwtProvider)
        {
            crypt = new CryptPass();
            JwtProvider = _JwtProvider;
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(UserRegistrationModel user)
        {
            using (DbUser db = new DbUser())
            {
                if (ModelState.IsValid)
                {
                    if (user != null && user.isAgree)
                    {
                        if(db.UserInfo.Any(u => u.Mail == user.Mail))
                        {
                            ModelState.AddModelError("Mail", "A user with this email already exists.");
                            return View(user);
                        }
                        user.Password = crypt.Encode(user.Password);
                        user.ConfirmPassword = crypt.Encode(user.ConfirmPassword);
                        db.UserInfo.Add(user);
                        db.SaveChanges();
                        return View("Accept", user);
                    }
                    else
                    {
                        //Изм на не аутентиф
                        return NotFound();
                    }
                }
                else
                {
                    return View(user);
                }
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel users)
        {
            if (ModelState.IsValid)
            {
                UserRegistrationModel user =  null;
                using(DbUser db = new DbUser())
                {
                    users.Password = crypt.Encode(users.Password);
                    user = db.UserInfo.FirstOrDefault(x => x.Mail == users.Mail && x.Password == users.Password);
                }
                if (user != null)
                {
                    var token = JwtProvider.GenerateToken(users);
                    HttpContext.Response.Cookies.Append("tasty-cookies", token);
                    return View("AcceptLogin", user);
                }
            }
            return NotFound();
        }

        //TODO
        //Сделать так, чтобы нельзя было регать двух пользователей с одной почтой
        //И сделать сессию, чтобы ты зашёл и был на сайте, пока не выйдешь из профиля
    }
}
 