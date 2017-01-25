using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Models.HomeViewModels
{
    public class SearchViewModel
    {
        public Book Book { get; set; }

        public User User { get; set; }
    }
}
