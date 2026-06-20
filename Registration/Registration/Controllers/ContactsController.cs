using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Registration.Context;
using Registration.Models;

namespace Registration.Controllers
{
    public class ContactsController : Controller
    {
        public IActionResult Contact()
        {
            using (DbUser db = new DbUser())
            {
                List<UserRegistrationModel> list = db.UserInfo.ToList();
                return View(list);
            }
        }
    }
}
