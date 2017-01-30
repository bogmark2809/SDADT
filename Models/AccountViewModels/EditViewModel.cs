using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models.AccountViewModels
{
    public class EditViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Id")]
        public string Id { get; set; }

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

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 12)]
        [Display(Name = "Personal number")]
        public string PersonalNumber { get; set; }

        public int LoanLimit { get; set; }
    }
}
