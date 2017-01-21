using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class Book
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        [DataType(DataType.MultilineText)]
        public string Anotation { get; set; }
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
        [DataType(DataType.Currency)]        
        public decimal Price { get; set; }
        public int Count { get; set; }
        [Display(Name = "Available")]    
        public bool isAvailable { get; set; }
        [Display(Name = "In storage")]
        public bool isInStorage { get; set; }
        public List<Loan> Loans { get; set; }
    }
}