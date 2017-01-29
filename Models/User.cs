using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApp.Models
{
    public class User : IdentityUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DisplayFormat(DataFormatString = "{0:0000000000}", ApplyFormatInEditMode = true)]
        public int? RFID { get; set; }
        [MaxLength(55)]
        [StringLength(50, MinimumLength = 3)]
        public string Firstname { get; set; }
        [MaxLength(55)]
        [StringLength(50, MinimumLength = 3)]
        public string Lastname { get; set; }
        public int LoanLimit { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(12, MinimumLength = 12)]
        public string PersonalNumber { get; set; }
        public ICollection<Loan> Loans { get; set; }
    }
}
