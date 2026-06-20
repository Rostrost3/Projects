using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Entities
{
    public class Cart
    {
        public int Id { get; set; }

        public decimal TotalPrice => CartItems.Sum(x => x.Product?.Price * x.Quantity ?? 0);

        public int UserId { get; set; }

        public User User { get; set; }

        public List<CartItem> CartItems { get; set; } = new();
    }
}
