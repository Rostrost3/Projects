using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTO.Entities
{
    public class RegModel
    {
        [Required]
        [MinLength(3, ErrorMessage = "Min Length of Name is 3 symbols")]
        [MaxLength(30, ErrorMessage = "Max Length of Name is 30 symbols")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword")]
        [MinLength(5, ErrorMessage = "Min Length of Password is 5 symbols")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
