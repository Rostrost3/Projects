using Database.Entities;
using Database.Repositories;
using DTO.DTOEntities;
using DTO.Profiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IRepository<Cart> _rep;

        private readonly IAppMappingProfile _appMappingProfile;

        public CartController(IRepository<Cart> rep, IAppMappingProfile appMappingProfile)
        {
            _rep = rep;
            _appMappingProfile = appMappingProfile;
        }

        [HttpPost("addnew")]
        public ActionResult CreateCart()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            Cart cart = new Cart() 
            {
                UserId = userId
            };
            _rep.Add(cart);
            return Ok();
        }
    }
}
