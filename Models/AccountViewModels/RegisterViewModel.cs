using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Firstname")]
        public string Firstname { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Lastname")]
        public string Lastname { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Role")]
        public string Role { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 12)]
        [Display(Name = "Personal number")]
        public string PersonalNumber { get; set; }

        public int LoanLimit { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
