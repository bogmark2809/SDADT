using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Models.HomeViewModels
{
    public class IndexViewModel
    {
        public List<Book> LastBooks { get; set; }

        public List<Loan> LastLoans { get; set; }
    }
}
