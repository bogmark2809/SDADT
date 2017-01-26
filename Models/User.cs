using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApp.Models
{
    public class User : IdentityUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayFormat(DataFormatString = "{0:0000000000}", ApplyFormatInEditMode = true)]
        public int? RFID { get; set; }
        [MaxLength(55)]
        [StringLength(50, MinimumLength = 3)]
        public string Firstname { get; set; }
        [MaxLength(55)]
        [StringLength(50, MinimumLength = 3)]
        public string Lastname { get; set; }
        public int LoanLimit { get; set; }
        [DisplayFormat(DataFormatString = "{000000-00000}", ApplyFormatInEditMode = true)]
        public int PersonalNumber { get; set; }
        public List<Loan> Loans { get; set; }
    }
}
