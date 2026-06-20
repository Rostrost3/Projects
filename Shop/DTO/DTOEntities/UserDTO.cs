using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.DTOEntities
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public int RoleId { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
