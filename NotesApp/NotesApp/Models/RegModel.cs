using System.ComponentModel.DataAnnotations;

namespace NotesApp.Models
{
    public class RegModel
    {
        public int Id { get; set; }

        [Display(Description = "UserName")]
        [Required(ErrorMessage = "Enter UserName")]
        public string UserName { get; set; }

        [Display(Description = "Email")]
        [Required(ErrorMessage = "Enter your email")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Description = "Password")]
        [Required(ErrorMessage = "Enter password")]
        [UIHint("Password")]
        [Compare("ConfirmPassword")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "The password could be from 5 to 20 symbols")]
        public string Password { get; set; }

        [Display(Description = "Confirm password")]
        [Required(ErrorMessage = "Enter password")]
        [UIHint("Password")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "The password could be from 5 to 20 symbols")]
        public string ConfirmPassword { get; set; }
    }
}
