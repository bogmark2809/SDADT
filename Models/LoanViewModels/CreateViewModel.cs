using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models.LoanViewModels
{
    public class CreateViewModel
    {
        [Required]
        [Display(Name = "RFID")]
        [DisplayFormat(DataFormatString = "{0:0000000000}", ApplyFormatInEditMode = true)]
        public int RFID { get; set; }

        [Required]
        [Display(Name = "Book Code")]
        [DisplayFormat(DataFormatString = "{0:0000000000}", ApplyFormatInEditMode = true)]
        public int Code { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Return date")]
        public DateTime ReturnDate { get; set; }

        [Display(Name = "Is returned?")]
        public bool isReturned { get; set; }
    }
}
