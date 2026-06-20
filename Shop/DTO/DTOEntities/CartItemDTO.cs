using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.DTOEntities
{
    public class CartItemDTO
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public int CartId { get; set; }
    }
}
