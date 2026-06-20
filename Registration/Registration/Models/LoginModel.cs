using System.ComponentModel.DataAnnotations;

namespace Registration.Models
{
    public class LoginModel
    {
        [Display(Name = "Email-Address")]
        [UIHint("EmailAddress")]
        [EmailAddress]
        [Required(ErrorMessage = "Please enter your email")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid")]
        public string Mail { get; set; }

        [Display(Name = "Password")]
        [UIHint("Password")]
        [Required(ErrorMessage = "Please enter your password")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "Your password doesn't fall within the range of 6 to 16 characters")]
        public string Password { get; set; }
    }
}
