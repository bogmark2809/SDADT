using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApp.Models
{
    public class Book
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayFormat(DataFormatString = "{0:0000000000}", ApplyFormatInEditMode = true)]
        [Display(Name = "Book code")]
        public int? Id { get; set; }
        [MaxLength(55)]
        [StringLength(50, MinimumLength = 5)]
        public string Title { get; set; }
        [MaxLength(55)]
        [StringLength(50, MinimumLength = 5)]
        public string Author { get; set; }
        [MaxLength(255)]
        [StringLength(250)]
        public string Anotation { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Publish date")]
        public DateTime ReleaseDate { get; set; }
        [MaxLength(25)]
        [StringLength(20, MinimumLength = 5)]
        public string Genre { get; set; }
        [DataType(DataType.Currency)]        
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        [Display(Name = "Available")]    
        public bool isAvailable { get; set; }
        [Display(Name = "In storage")]
        public bool isInStorage { get; set; }
        public ICollection<Loan> Loans { get; set; }
    }
}