using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LibraryApp.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class User : IdentityUser
    {
        [DisplayFormat(DataFormatString = "{0:0000000000}", ApplyFormatInEditMode = true)]
        public int RFID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int LoanLimit { get; set; }
        public string PersonalNumber { get; set; }
        public List<Loan> Loans { get; set; }
    }
}
