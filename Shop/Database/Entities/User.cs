using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Database.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public int RoleId { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public Cart Cart { get; set; }

        public Role Role { get; set; }
    }
}
