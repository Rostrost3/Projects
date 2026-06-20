using System.ComponentModel.DataAnnotations;

namespace NotesApp.Models
{
    public class LoginModel
    {
        [Display(Description = "Mail")]
        [Required(ErrorMessage = "Enter your Mail")]
        public string Email { get; set; }

        [Display(Description = "Password")]
        [Required(ErrorMessage = "Enter your password")]
        [UIHint("Password")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "The password could be from 5 to 20 symbols")]
        public string Password { get; set; }
    }
}