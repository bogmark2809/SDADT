using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models.LoanViewModels
{
    public class EditViewModel
    {
        [Required]
        public int? id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Return date")]
        public DateTime ReturnDate { get; set; }

        [Display(Name = "Is returned?")]
        public bool isReturned { get; set; }
    }
}
