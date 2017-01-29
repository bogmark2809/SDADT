using System.Collections.Generic;

namespace LibraryApp.Models.HomeViewModels
{
    public class SearchViewModel
    {
        public bool isForBookList { get; set; }
        public ICollection<Book> Book { get; set; }

        public User User { get; set; }
    }
}
