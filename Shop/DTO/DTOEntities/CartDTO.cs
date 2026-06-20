using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.DTOEntities
{
    public class CartDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
