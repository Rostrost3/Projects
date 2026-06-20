using Database.Entities;
using Database.Repositories;
using DTO.DTOEntities;
using DTO.Profiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartItemController : Controller
    {
        private readonly IRepository<CartItem> _rep;

        private readonly ICartService _cartService;

        private readonly IAppMappingProfile _appMappingProfile;

        public CartItemController(IRepository<CartItem> rep, IAppMappingProfile appMappingProfile, ICartService cartService)
        {
            _rep = rep;
            _appMappingProfile = appMappingProfile;
            _cartService = cartService;
        }

        [HttpGet("getall")]
        public ActionResult<List<CartItemDTO>> GetAll()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            int cartId = _cartService.GetCartIdByUserId(userId);
            var res = _rep.GetAllByFunc(x => x.CartId == cartId).Select(x => _appMappingProfile.CartItemToCartItemDTO(x)).ToList();
            return Ok(res);
        }

        [HttpGet("getbyid/{id:int}")]
        public ActionResult<CartItemDTO> GetById(int id)
        {
            return Ok(_appMappingProfile.CartItemToCartItemDTO(_rep.GetByFunc(x => x.Id == id)!));
        }

        [HttpPost("addnew")]
        public ActionResult Add([FromBody] CartItemDTO cartItemDTO)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            int cartId = _cartService.GetCartIdByUserId(userId);
            cartItemDTO.CartId = cartId;
            CartItem cartItem = _appMappingProfile.CartItemDTOToCartItem(cartItemDTO);
            _rep.Add(cartItem);
            return CreatedAtAction(nameof(GetById), new { id = cartItem.Id }, cartItemDTO);
        }

        [HttpPut("update")]
        public ActionResult Update([FromBody] CartItemDTO cartItemDTO)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var item = _rep.GetByFunc(x => x.Id == cartItemDTO.Id && x.Cart.UserId == userId);
            if (item != null)
            {
                CartItem cartItem = _appMappingProfile.CartItemDTOToCartItem(cartItemDTO);
                _rep.Update(cartItem);
            }
            return NoContent();
        }

        [HttpDelete("delete/{id:int}")]
        public ActionResult Delete(int id)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var item = _rep.GetByFunc(x => x.Id == id && x.Cart.UserId == userId);
            if (item != null)
            {
                _rep.Delete(item);
            }
            return NoContent();
        }
    }
}
