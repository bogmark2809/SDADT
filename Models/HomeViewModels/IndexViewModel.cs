using System.Collections.Generic;

namespace LibraryApp.Models.HomeViewModels
{
    public class IndexViewModel
    {
        public List<Book> LastBooks { get; set; }

        public List<Loan> LastLoans { get; set; }
    }
}
