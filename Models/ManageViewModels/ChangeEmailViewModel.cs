using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models.ManageViewModels
{
    public class ChangeEmailViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Current email")]
        public string OldEmail { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "New email")]
        public string NewEmail { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Confirm new email")]
        [Compare("NewEmail", ErrorMessage = "The new email and confirmation email do not match.")]
        public string ConfirmEmail { get; set; }
    }
}
