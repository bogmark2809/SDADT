using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace LibraryApp.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public string Email { get; set; }
        [DisplayFormat(DataFormatString = "{0:0000000000}", ApplyFormatInEditMode = true)]
        public int? RFID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int LoanLimit { get; set; }
        public string PersonalNumber { get; set; }
        public string Role { get; set; }
        public ICollection<Loan> Loans { get; set; }
        public bool HasPassword { get; set; }

        public IList<UserLoginInfo> Logins { get; set; }

        public string PhoneNumber { get; set; }

        public bool TwoFactor { get; set; }

        public bool BrowserRemembered { get; set; }
    }
}
