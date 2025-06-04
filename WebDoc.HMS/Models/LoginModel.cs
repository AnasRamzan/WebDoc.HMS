using System.ComponentModel.DataAnnotations;

namespace WebDoc.HMS.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = "";

        //public bool RememberMe { get; set; }
    }
}
