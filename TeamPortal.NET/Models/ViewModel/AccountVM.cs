using System.ComponentModel.DataAnnotations;

namespace TeamPortal.NET.Models.ViewModel
{
    public class RegisterVM
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
    public class LoginVM
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
