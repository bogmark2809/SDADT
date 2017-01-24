using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Models.LoanViewModels
{
    public class CreateViewModel
    {
        [Required]
        [Display(Name = "RFID")]
        public int RFID { get; set; }

        [Required]
        [Display(Name = "Book Code")]
        public int Code { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Return Date")]
        public DateTime ReturnDate { get; set; }
    }
}
