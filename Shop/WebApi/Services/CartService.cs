using Database.Entities;
using Database.Repositories;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    public class CartService : ICartService
    {
        private readonly IRepository<Cart> _repCart;

        public CartService(IRepository<Cart> repCart)
        {
            _repCart = repCart;
        }

        public int GetCartIdByUserId(int userId)
        {
            return _repCart.GetByFunc(x => x.UserId == userId)!.Id;
        }
    }
}
