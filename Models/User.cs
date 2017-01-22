using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
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
        public List<Loan> Loans { get; set; }
    }
}
