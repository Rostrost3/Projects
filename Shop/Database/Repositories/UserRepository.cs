using Database.Context;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Repositories
{
    public class UserRepository : Repository<User>
    {
        private readonly ApplicationDBContext _context;

        public UserRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public User? GetUserWithFullCart(int id)
        {
            return _context.Users.Where(u => u.Id == id).Include(u => u.Cart).ThenInclude(c => c.CartItems).ThenInclude(ci => ci.Product).FirstOrDefault();
        }
    }
}
